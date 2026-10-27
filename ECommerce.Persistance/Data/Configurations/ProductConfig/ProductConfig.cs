using ECommerce.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistance.Data.Configurations.ProductConfig
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(P => P.Name)
                   .HasMaxLength(100);

            builder.Property(P => P.Description)
                .HasMaxLength(500);

            builder.Property(P => P.PictureUrl)
                .HasMaxLength(200);

            builder.Property(P => P.Price)
                .HasPrecision(18, 2);

            //Relationship Configurations
            builder.HasOne(P => P.ProductBrand)
                     .WithMany(P => P.Products)
                     .HasForeignKey(P => P.BrandId);

            builder.HasOne(P => P.ProductType)
                        .WithMany(P => P.Products)
                        .HasForeignKey(P => P.TypeId);
        }
    }
}
