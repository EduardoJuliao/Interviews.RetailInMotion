using Interviews.RetailInMotion.Domain.Entities;

namespace Interviews.RetailInMotion.Domain.Interfaces.Services
{
    public interface IStockService
    {
        Task<Product> SecureProduct(Guid productId, int quantity);
        Task<Product> SecureProducts(IEnumerable<KeyValuePair<Guid, int>> productQuantity);
        Task ReturnProduct(Guid productId, int quantity);
        Task<Product> ReturnProducts(IEnumerable<KeyValuePair<Guid, int>> productQuantity);
    }
}
