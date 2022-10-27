using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Interfaces.Repositories;
using Interviews.RetailInMotion.Domain.Interfaces.Services;
using Interviews.RetailInMotion.Domain.Services;
using Interviews.RetailInMotion.Domain.Tests.Helpers;
using Interviews.RetailInMotion.Repository;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Domain.Tests.Service
{
    [Category("Service")]
    public class StockServiceTests
    {
        private ApplicationDbContext _context;
        private IStockRepository _stockRepository;
        private IStockService _stockService;

        [SetUp]
        public void SetUp()
        {
            _context = EntityFrameworkHelper.CreateDatabase();
            _stockRepository = new StockRepository(_context);

            _stockService = new StockService(Substitute.For<ILogger<StockService>>(), _stockRepository);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task CanSecureProductFromStock()
        {
            var stock = new Stock
            {
                Product = new Product
                {
                    Name = "Product 1",
                    Price = 10.76
                },
                QuantityAvailable = 10
            };

            _context.Stock.Add(stock);

            _context.SaveChanges();

            var result = await _stockService.SecureProduct(stock.ProductId, 3);

            Assert.AreEqual(_context.Stock.Single(x => x.ProductId == stock.ProductId).QuantityAvailable, 7);
        }

        [Test]
        public void ThrowExceptionIfProductIsNotFound()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _stockService.SecureProduct(Guid.NewGuid(), 0));
        }

        [Test]
        public void ThrowExceptionIfRequiredAmountOfItemsIfBiggerThanAmountInStock()
        {
            var stock = new Stock
            {
                Product = new Product
                {
                    Name = "Product 1",
                    Price = 10.76
                },
                QuantityAvailable = 2
            };

            _context.Stock.Add(stock);

            _context.SaveChanges();

            Assert.ThrowsAsync<IndexOutOfRangeException>(async () =>
            {
                await _stockService.SecureProduct(stock.ProductId, 3);
            });
        }

        [Test]
        public async Task CanReturnProductToStock()
        {
            var stock = new Stock
            {
                Product = new Product
                {
                    Name = "Product 1",
                    Price = 10.76
                },
                QuantityAvailable = 0
            };

            _context.Stock.Add(stock);

            _context.SaveChanges();

            await _stockService.ReturnProduct(stock.ProductId, 5);

            Assert.AreEqual(5, _context.Stock.Single(x => x.ProductId == stock.ProductId).QuantityAvailable);
        }
    }
}
