using Interviews.RetailInMotion.Domain.Entities;

namespace Interviews.RetailInMotion.Domain.Interfaces.Repositories
{
    public interface IStockRepository
    {
        Task<Product> SecureProduct(Guid productId, int quantity);
        Task ReturnProductToStock(Guid productId, int quantity);
    }
}
