using Interviews.RetailInMotion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            builder.Property(x => x.LastUpdatedDate)
                .IsRequired(false);

            builder.Property(x => x.CanceledDate)
                .IsRequired(false);

            builder.Property(x => x.BillingAddressSameAsDelivery)
                .IsRequired()
                .HasDefaultValue(false);

            //builder.HasOne(x => x.DeliveryAddress)
            //    .WithMany(x => x.Orders)
            //    .HasForeignKey(x => x.DeliveryAddressId);

            //builder.HasOne(x => x.BillingAddress)
            //    .WithMany(x => x.Orders)
            //    .HasForeignKey(x => x.BillingAddressId);

            builder.Ignore(x => x.OrderProducts);

            builder.Ignore(x => x.TotalPrice);
        }
    }
}
