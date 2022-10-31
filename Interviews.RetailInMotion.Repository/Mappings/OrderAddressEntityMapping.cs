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
    internal class OrderAddressEntityMapping : IEntityTypeConfiguration<OrderAddress>
    {
        public void Configure(EntityTypeBuilder<OrderAddress> builder)
        {
            builder.HasKey(x => new { x.OrderId, x.AddressId });

            builder.HasOne(x => x.Address)
                .WithMany(x => x.OrderAddresses)
                .HasForeignKey(x => x.AddressId);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderAddresses)
                .HasForeignKey(x => x.OrderId);
        }
    }
}
