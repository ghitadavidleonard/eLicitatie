using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Api.Entities.TypeConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(pt => pt.Id);

            builder.Property(pt => pt.ShortDescription)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(pt => pt.LongDescription)
                .IsRequired()
                .HasColumnType("nvarchar(255)");

            builder.Property(pt => pt.Image)
                .HasColumnType("varbinary(MAX)");

            builder.Property(pt => pt.OwnerId)
                .IsRequired()
                .HasColumnType("int");

            builder.ToTable("Products");
        }
    }
}
