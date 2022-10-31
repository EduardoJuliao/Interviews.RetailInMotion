using Interviews.RetailInMotion.Domain.Entities;

namespace Interviews.RetailInMotion.Domain.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders(int take = 20, int skip = 0);
        Task<Order> GetOrder(Guid id);
        Task CancelOrder(Guid orderId);
        Task<Order> CreateOrder(Order order);
        Task<Order> UpdateOrder(Order order);
    }
}
