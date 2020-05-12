using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Api.Entities.TypeConfiguration
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(pc => pc.Id);

            builder.Property(pc => pc.CategoryId)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(pc => pc.ProductId)
                .IsRequired()
                .HasColumnType("int");

            builder.ToTable("ProductCategories");
        }
    }
}
