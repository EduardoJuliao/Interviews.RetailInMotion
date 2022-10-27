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
    internal class OrderEntityMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValue(Guid.NewGuid());

            builder.Property(x => x.CreationDate)
                .HasDefaultValue(DateTimeOffset.Now)
                .IsRequired();

            builder.Property(x => x.LastUpdated)
                .HasDefaultValue(DateTimeOffset.Now)
                .IsRequired();

            builder.Property(x => x.DeliveryAddressSameAsBilling)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Ignore(x => x.DeliveryAddress);
            builder.Ignore(x => x.BillingAddress);
            builder.Ignore(x => x.OrderItems);

            builder.Ignore(x => x.TotalPrice);
        }
    }
}
