using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Domain.Services
{
    public class OrderService : IOrderService
    {
        public Task<Order> CancelOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> CreateOrder()
        {
            throw new NotImplementedException();
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
