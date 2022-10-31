using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Enums;
using Interviews.RetailInMotion.Domain.EventArgs.Order;
using Interviews.RetailInMotion.Domain.Extensions;
using Interviews.RetailInMotion.Domain.Interfaces.Factories;
using Interviews.RetailInMotion.Domain.Interfaces.Repositories;
using Interviews.RetailInMotion.Domain.Interfaces.Services;
using Interviews.RetailInMotion.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Interviews.RetailInMotion.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IOrderFactory _orderFactory;
        private readonly IOrderRepository _orderRepository;
        private readonly IStockService _stockService;

        public delegate void OrderCanceledEventHandler(object sender, OrderCanceledEventArgs e);
        public event OrderCanceledEventHandler OrderCanceledEvent;

        public delegate void OrderCreatedEventHandler(object sender, OrderCreatedEventArgs e);
        public event OrderCreatedEventHandler OrderCreatedEvent;

        public OrderService(
            ILogger<OrderService> logger,
            IOrderFactory orderFactory,
            IOrderRepository orderRepository,
            IStockService stockService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _orderFactory = orderFactory ?? throw new ArgumentNullException(nameof(orderFactory));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _stockService = stockService ?? throw new ArgumentNullException(nameof(stockService));
        }

        public async Task<Order> CancelOrder(Guid orderId)
        {
            var order = await EnsureOrderIsValidToUpdate(orderId);

            order.Status = OrderStatus.Canceled;
            order.LastUpdatedDate = DateTimeOffset.UtcNow;
            order.CanceledDate = DateTimeOffset.UtcNow;

            await _orderRepository.UpdateOrder(order);

            OrderCanceledEvent?.Invoke(this, new OrderCanceledEventArgs(order));

            return order;
        }

        public async Task<Order> CreateOrder(CreateOrderModel orderModel)
        {
            var newOrder = _orderFactory
                .AddBillingAddress(orderModel.BillingAddress)
                .AddDeliveryAddress(orderModel.DeliveryAddress)
                .BillingAddressSameAsDelivery(orderModel.BillingAddressSameAsDelivery)
                .AddProducts(orderModel.Products)
                .Build();

            var productAvailability = await _stockService.HasProductAvailability(newOrder.OrderProducts);
            if (!productAvailability.productsAvailable)
                throw new Exception("Some Products aren't available in the desired quantity!" +
                    Environment.NewLine + "List of Products:" + Environment.NewLine +
                    String.Join(Environment.NewLine, productAvailability.unavailableProductIds));

            await _stockService
                .SecureProducts(newOrder.OrderProducts.Select(x => new KeyValuePair<Guid, int>(x.ProductId, x.Quantity)));

            var order = await _orderRepository.CreateOrder(newOrder);

            OrderCreatedEvent?.Invoke(this, new OrderCreatedEventArgs(order));

            return order;
        }

        public async Task<Order> GetOrder(Guid id)
        {
            return await _orderRepository.GetOrder(id);
        }

        public async Task<IEnumerable<Order>> GetOrders(int take = 20, int skip = 0)
        {
            return await _orderRepository.GetOrders(take, skip);
        }

        public async Task<Order> UpdateAddress(Guid orderId, UpdateAddressModel addressModel)
        {
            var order = await EnsureOrderIsValidToUpdate(orderId);

            _orderFactory
                .BasedOnOrder(order)
                .AddDeliveryAddress(addressModel.DeliveryAddress)
                .AddBillingAddress(addressModel.BillingAddress)
                .BillingAddressSameAsDelivery(addressModel.BillingAddressSameAsDelivery)
                .Build();

            order.LastUpdatedDate = DateTimeOffset.UtcNow;

            return await _orderRepository.UpdateOrder(order);
        }

        public async Task<Order> UpdateOrderProducts(Guid orderId, IEnumerable<CreateOrderProductModel> products)
        {
            var tran = await _orderRepository.BeginTransactionAsync();
            try
            {
                var order = await EnsureOrderIsValidToUpdate(orderId);

                var updatedProducts = order.OrderProducts.Where(x => products.Select(p => p.ProductId).Contains(x.ProductId)).ToList();
                var deletedProducts = (from op in order.OrderProducts
                                       where !products.Any(p => (p.ProductId == op.ProductId))
                                       select op).ToList();
                var addedProducts = (from p in products
                                     where !order.OrderProducts.Any(op => (op.ProductId == p.ProductId))
                                     select p).ToList();

                foreach (var existingUpdatedProduct in updatedProducts)
                {
                    var updatedProduct = products.Single(x => x.ProductId == existingUpdatedProduct.ProductId);

                    if (existingUpdatedProduct.Quantity > updatedProduct.Quantity)
                        await _stockService.ReturnProduct(existingUpdatedProduct.ProductId,
                            existingUpdatedProduct.Quantity - updatedProduct.Quantity);
                    else
                        await _stockService.SecureProduct(existingUpdatedProduct.ProductId,
                            updatedProduct.Quantity - existingUpdatedProduct.Quantity);

                    existingUpdatedProduct.Quantity = updatedProduct.Quantity;
                }

                foreach (var deletedProduct in deletedProducts)
                {
                    var index = order.OrderProducts
                        .Single(x => x.ProductId == deletedProduct.ProductId);
                    order.OrderProducts.Remove(index);
                    await _stockService.ReturnProduct(deletedProduct.ProductId, deletedProduct.Quantity);
                }

                foreach (var addedProduct in addedProducts)
                {
                    order.OrderProducts.Add(new OrderProduct
                    {
                        ProductId = addedProduct.ProductId,
                        Quantity = addedProduct.Quantity,
                        Order = order
                    });
                    await _stockService.SecureProduct(addedProduct.ProductId, addedProduct.Quantity);
                }

                await _orderRepository.UpdateOrder(order);
                await tran.CommitAsync();

                return order;
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                throw;
            }

        }

        private async Task<Order> EnsureOrderIsValidToUpdate(Guid orderId)
        {
            var order = await _orderRepository.GetOrder(orderId);

            if (order == null)
                throw new Exception("Order not found.");

            if (order.Status != OrderStatus.Created && order.Status != OrderStatus.WaitingDeparture)
                throw new Exception("Cannot change order address as has already in delivery");

            return order;
        }
    }
}
