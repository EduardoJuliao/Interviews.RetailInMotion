using Interviews.RetailInMotion.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interviews.RetailInMotion.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
