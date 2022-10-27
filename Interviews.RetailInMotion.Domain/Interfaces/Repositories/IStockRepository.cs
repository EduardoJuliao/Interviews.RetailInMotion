using Interviews.RetailInMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Domain.Interfaces.Repositories
{
    public interface IStockRepository
    {
        Task<Product> SecureProduct(Guid productId, int quantity);
        Task ReturnProductToStock(Guid productId, int quantity);
    }
}
