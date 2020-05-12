using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Api.Entities.TypeConfiguration
{
    public class OfferConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.UserId)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(o => o.AuctionId)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(o => o.Price)
                .IsRequired()
                .HasColumnType("decimal(18,0)");

            builder.ToTable("Offers");
        }
    }
}
