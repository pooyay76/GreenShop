using System.Net.Http.Headers;
using GreenShop.Mappings;
using GreenShop.Models;
using Microsoft.EntityFrameworkCore;
namespace GreenShop.Contexts
{
    public class ShopContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public ShopContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}
