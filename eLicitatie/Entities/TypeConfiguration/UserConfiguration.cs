using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Api.Entities.TypeConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(n => n.FirstName)
                .IsRequired()
                .HasColumnType("nvarchar(120)");

            builder.Property(n => n.LastName)
                .IsRequired()
                .HasColumnType("nvarchar(120)");

            builder.Property(n => n.Role)
                .IsRequired()
                .HasColumnType("int")
                .HasDefaultValueSql("1");

            builder.Property(n => n.Email)
                .IsRequired()
                .HasColumnType("nvarchar(255)");

            builder.Property(n => n.PasswordHash)
                .IsRequired()
                .HasColumnType("nvarchar(255)");

            builder.ToTable("Users");
        }
    }
}
