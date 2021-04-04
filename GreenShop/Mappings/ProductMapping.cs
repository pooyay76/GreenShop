using System.Security.Cryptography.X509Certificates;
using GreenShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace GreenShop.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(model => model.Id);
            builder.Property(model => model.Name).HasMaxLength(255).IsRequired();
            builder.Property(model => model.Caption).HasMaxLength(1000).IsRequired();
            builder.Property(model => model.ImageUrl).HasMaxLength(500);
            builder.Property(model => model.Stock).HasMaxLength(20).IsRequired();
            builder.Property(model => model.Price).HasMaxLength(20).IsRequired();
            builder.Property(model => model.IsDeleted).HasDefaultValue(false);
        }
    }
}