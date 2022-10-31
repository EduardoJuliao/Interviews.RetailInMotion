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

            builder.Ignore(x => x.TotalPrice);
        }
    }
}
