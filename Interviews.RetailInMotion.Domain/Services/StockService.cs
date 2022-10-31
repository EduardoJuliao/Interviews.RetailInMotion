using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Interfaces.Repositories;
using Interviews.RetailInMotion.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Interviews.RetailInMotion.Domain.Services
{
    public class StockService : IStockService
    {
        private readonly ILogger<StockService> _logger;
        private readonly IStockRepository _stockRepository;

        public StockService(
            ILogger<StockService> logger,
            IStockRepository stockRepository)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (stockRepository is null)
            {
                throw new ArgumentNullException(nameof(stockRepository));
            }

            this._logger = logger;
            this._stockRepository = stockRepository;
        }

        public async Task<(bool productsAvailable, List<Guid> unavailableProductIds)> HasProductAvailability(IEnumerable<OrderProduct> productsInOrder)
        {
            var unavailableProductIds = new List<Guid>();

            //TODO: This can be further improved byu using joins instead of using for
            foreach(var orderProduct in productsInOrder)
            {
                var isProductAvailable = await _stockRepository.IsProductAvailable(orderProduct.ProductId, orderProduct.Quantity);
                if (!isProductAvailable)
                    unavailableProductIds.Add(orderProduct.ProductId);
            }

            return (!unavailableProductIds.Any(), unavailableProductIds);
        }

        public async Task ReturnProduct(Guid productId, int quantity)
        {
            await _stockRepository.ReturnProductToStock(productId, quantity);
        }

        public Task ReturnProducts(IEnumerable<KeyValuePair<Guid, int>> productQuantity)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> SecureProduct(Guid productId, int quantity)
        {
            return await _stockRepository.SecureProduct(productId, quantity);
        }

        public async Task SecureProducts(IEnumerable<KeyValuePair<Guid, int>> productQuantity)
        {
            foreach (var product in productQuantity)
                await SecureProduct(product.Key, product.Value);
        }
    }
}
