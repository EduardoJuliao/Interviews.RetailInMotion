using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public OrderRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Order> GetOrder(Guid id)
        {
            return await _applicationDbContext.Orders.SingleAsync(x => x.Id == id);
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
