using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Models;
using static Interviews.RetailInMotion.Domain.Services.OrderService;

namespace Interviews.RetailInMotion.Domain.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Order> GetOrder(Guid id);
        Task<IEnumerable<Order>> GetOrders(int take = 20, int skip = 0);
        Task<Order> CreateOrder(CreateOrderModel orderModel);
        Task<Order> UpdateAddress(Guid orderId, UpdateAddressModel addressModel);
        Task<Order> UpdateOrderProducts(Guid orderId, IEnumerable<CreateOrderProductModel> products);
        Task<Order> CancelOrder(Guid orderId);

        public event OrderCreatedEventHandler OrderCreatedEvent;
        public event OrderCanceledEventHandler OrderCanceledEvent;

    }
}
