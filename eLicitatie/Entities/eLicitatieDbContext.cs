using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace eLicitatie.Api.Entities
{
    public class eLicitatieDbContext : DbContext
    {
        public eLicitatieDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users{ get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Offer> Offers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Creator).WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Product).WithOne().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Auction>()
                .HasMany(o => o.Offers).WithOne(a => a.Auction).IsRequired().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(a => a.Category).WithMany(m => m.ProductCategories).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Category>()
                .HasMany(a => a.ProductCategories).WithOne(p => p.Category).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Offer>()
                .HasOne(o => o.Auction).WithMany(a => a.Offers).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Offer>()
                .HasOne(u => u.User).WithMany().OnDelete(DeleteBehavior.NoAction);

        }
    }
}
