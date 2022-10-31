using Interviews.RetailInMotion.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Repository
{
    public class BaseRepository : IBaseRepository
    {
        protected readonly ApplicationDbContext _applicationDbContext;

        public BaseRepository(ApplicationDbContext context)
        {
            _applicationDbContext = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _applicationDbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _applicationDbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _applicationDbContext.Database.RollbackTransactionAsync();
        }
    }
}
