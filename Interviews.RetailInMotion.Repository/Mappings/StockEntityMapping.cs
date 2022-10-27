using Interviews.RetailInMotion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviews.RetailInMotion.Repository.Mappings
{
    internal class StockEntityMapping : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("Stock");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValue(Guid.NewGuid());

            builder.Property(x => x.QuantityAvailable)
                .IsRequired()
                .HasDefaultValue(0);

            builder
                .HasOne(x => x.Product)
                .WithOne(x => x.Stock)
                .HasForeignKey<Stock>(x => x.ProductId);
        }
    }
}
