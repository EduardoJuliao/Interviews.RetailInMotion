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

        public async Task ReturnProduct(Guid productId, int quantity)
        {
            await _stockRepository.ReturnProductToStock(productId, quantity);
        }

        public Task<Product> ReturnProducts(IEnumerable<KeyValuePair<Guid, int>> productQuantity)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> SecureProduct(Guid productId, int quantity)
        {
            return await _stockRepository.SecureProduct(productId, quantity);
        }

        public Task<Product> SecureProducts(IEnumerable<KeyValuePair<Guid, int>> productQuantity)
        {
            throw new NotImplementedException();
        }
    }
}
