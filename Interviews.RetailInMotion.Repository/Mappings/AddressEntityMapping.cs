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
    internal class AddressEntityMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValue(Guid.NewGuid());

            builder.Property(x => x.Street)
                .IsRequired()
                .HasMaxLength(Address.StreetMaxLenght);

            builder.Property(x => x.PostalCode)
                .IsRequired();
        }
    }
}
