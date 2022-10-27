using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Interviews.RetailInMotion.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public StockRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Product> SecureProduct(Guid productId, int quantity)
        {
            var availableProduct = await _applicationDbContext
                .Stock
                .SingleOrDefaultAsync(x => x.ProductId == productId);

            if (availableProduct == null)
                throw new KeyNotFoundException("Product not found");

            if (availableProduct.QuantityAvailable - quantity < 0)
            {
                await _applicationDbContext.Database.RollbackTransactionAsync();
                throw new IndexOutOfRangeException(
                    $"{nameof(quantity)} informed is higher than the amount of items in stock. " +
                    $"Quantity Required {quantity}" +
                    $"Quantity Available {availableProduct.QuantityAvailable}");
            }

            availableProduct.QuantityAvailable -= quantity;

            await _applicationDbContext.SaveChangesAsync();

            return availableProduct.Product;
        }

        public async Task ReturnProductToStock(Guid productId, int quantity)
        {
            var availableProduct = await _applicationDbContext
                .Stock
                .SingleOrDefaultAsync(x => x.ProductId == productId);

            if (availableProduct == null)
                throw new KeyNotFoundException("Product isn't available");

            
            if (availableProduct.QuantityAvailable + quantity < 0)
            {
                await _applicationDbContext.Database.RollbackTransactionAsync();
                throw new IndexOutOfRangeException($"Added quantity leads to less than 0 items, check {nameof(quantity)} value. Received {quantity}");
            }

            availableProduct.QuantityAvailable += quantity;

            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
