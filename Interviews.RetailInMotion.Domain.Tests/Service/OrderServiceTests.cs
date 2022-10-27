using Microsoft.Extensions.Logging;
using Interviews.RetailInMotion.Domain.Interfaces.Services;
using Interviews.RetailInMotion.Domain.Services;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interviews.RetailInMotion.Repository;
using Interviews.RetailInMotion.Domain.Tests.Helpers;
using Interviews.RetailInMotion.Domain.Interfaces.Repositories;
using Interviews.RetailInMotion.Domain.Entities;

namespace Interviews.RetailInMotion.Domain.Tests.Service
{
    [Category("Service")]
    public class OrderServiceTests
    {
        private ApplicationDbContext _context;
        private IOrderRepository _orderRepository;
        private IOrderService _orderService;

        [SetUp]
        public void SetUp()
        {
            _context = EntityFrameworkHelper.CreateDatabase();
            _orderRepository = new OrderRepository(_context);

            _orderService = new OrderService(Substitute.For<ILogger<OrderService>>(), _orderRepository);
        }

        [Test]
        public async Task GetLatestOrdersCreated()
        {
            var firstDate = DateTimeOffset.Now;

            _context.Orders.AddRange(
                Enumerable.Range(0, 50).Select(x => new Order
                {
                    CreationDate = firstDate.AddDays(x)
                }));

            await _context.SaveChangesAsync();

            var orders = await _orderService.GetOrders();

            Assert.That(orders.Count, Is.EqualTo(20));
            Assert.That(orders.First().CreationDate, Is.EqualTo(firstDate.AddDays(49)));
        }

        [Test]
        public async Task CanCreateOrder()
        {

        }
    }
}
