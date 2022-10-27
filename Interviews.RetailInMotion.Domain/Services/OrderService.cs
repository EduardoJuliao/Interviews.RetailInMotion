using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Interfaces.Repositories;
using Interviews.RetailInMotion.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderService(
            ILogger<OrderService> logger, 
            IOrderRepository orderRepository)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (orderRepository is null)
            {
                throw new ArgumentNullException(nameof(orderRepository));
            }

            this._logger = logger;
            this._orderRepository = orderRepository;
        }

        public Task<Order> CancelOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> CreateOrder()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrder(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetOrders(int take = 20, int skip = 0)
        {
            return await _orderRepository.GetOrders(take, skip);
        }

        public Task<Order> UpdateAddress()
        {
            throw new NotImplementedException();
        }

        public Task<Order> UpdateOrderProducts(Guid orderId, IEnumerable<Product> products)
        {
            throw new NotImplementedException();
        }
    }
}
