using Interviews.RetailInMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Domain.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Order> GetOrder(Guid id);
        Task<IEnumerable<Order>> GetOrders(int take = 20, int skip = 0);
        Task<Order> CreateOrder();
        Task<Order> UpdateAddress();
        Task<Order> UpdateOrderProducts(Guid orderId, IEnumerable<Product> products);
        Task<Order> CancelOrder(Guid orderId);
        
    }
}
