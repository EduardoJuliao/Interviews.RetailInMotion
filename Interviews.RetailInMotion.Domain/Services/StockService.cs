using Interviews.RetailInMotion.Domain.Entities;
using Interviews.RetailInMotion.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Domain.Services
{
    public class StockService : IStockService
    {
        public Task ReturnProduct(Guid productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task<Product> ReturnProducts(IEnumerable<KeyValuePair<Guid, int>> productQuantity)
        {
            throw new NotImplementedException();
        }

        public Task<Product> SecureProduct(Guid productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task<Product> SecureProducts(IEnumerable<KeyValuePair<Guid, int>> productQuantity)
        {
            throw new NotImplementedException();
        }
    }
}
