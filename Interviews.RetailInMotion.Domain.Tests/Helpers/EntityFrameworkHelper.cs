using Interviews.RetailInMotion.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Interviews.RetailInMotion.Domain.Tests.Helpers
{
    internal static class EntityFrameworkHelper
    {
        public static ApplicationDbContext CreateDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("RetailUnitTests")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var context = new ApplicationDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        public static void DropDatabase(ApplicationDbContext context)
        {
            context.Dispose();
        }
    }
}
