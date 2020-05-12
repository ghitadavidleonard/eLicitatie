using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eLicitatie.Api.Entities.TypeConfiguration
{
    public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.StartingPrice)
                .IsRequired()
                .HasColumnType("decimal(18,0)");

            builder.Property(a => a.StartDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(a => a.DaysActive)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(a => a.CreatorId)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(a => a.ProductId)
                .IsRequired()
                .HasColumnType("int");

            builder.ToTable("Auctions");
        }
    }
}