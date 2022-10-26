using Interviews.RetailInMotion.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Repository
{
    public class StockRepository: IStockRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public StockRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

    }
}
