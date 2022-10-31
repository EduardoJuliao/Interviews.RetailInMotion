using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Interviews.RetailInMotion.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public OrderRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Task CancelOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> CreateOrder(Order order)
        {
            _applicationDbContext.Orders.Add(order);
            await _applicationDbContext.SaveChangesAsync();

            return order;
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            _applicationDbContext.Orders.Update(order);

            await _applicationDbContext.SaveChangesAsync();

            return order;
        }

        public async Task<Order> GetOrder(Guid id)
        {
            return await _applicationDbContext.Orders
                .Include(x => x.OrderAddresses)
                .SingleAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Order>> GetOrders(int take = 20, int skip = 0)
        {
            return await _applicationDbContext.Orders
                .OrderByDescending(x => x.CreationDate)
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }
    }
}
