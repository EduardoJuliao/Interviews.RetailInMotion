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
    internal class ProductEntityMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValue(Guid.NewGuid());

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(Product.NameMaxLength);

            builder.Property(x => x.Price)
                .IsRequired();

            builder
                .HasOne(x => x.Stock)
                .WithOne(x => x.Product)
                .HasForeignKey<Product>(x => x.StockId);
        }
    }
}
