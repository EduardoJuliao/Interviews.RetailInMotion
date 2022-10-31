using Interviews.RetailInMotion.Domain.Entities;

namespace Interviews.RetailInMotion.Domain.Interfaces.Services
{
    public interface IStockService
    {
        Task<Product> SecureProduct(Guid productId, int quantity);
        Task SecureProducts(IEnumerable<KeyValuePair<Guid, int>> productQuantity);
        Task ReturnProduct(Guid productId, int quantity);
        Task ReturnProducts(IEnumerable<KeyValuePair<Guid, int>> productQuantity);
        Task<(bool productsAvailable, List<Guid> unavailableProductIds)> HasProductAvailability(IEnumerable<OrderProduct> productsInOrder);
    }
}
