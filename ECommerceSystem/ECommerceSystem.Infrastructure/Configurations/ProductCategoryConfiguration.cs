using ECommerceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Infrastructure.Configurations
{
    internal class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(pc => new { pc.ProductId, pc.CategoryId });

            builder.HasOne(pc => pc.Product)
                   .WithMany()
                   .HasForeignKey(pc => pc.ProductId);

            builder.HasOne(pc => pc.Category)
                   .WithMany() 
                   .HasForeignKey(pc => pc.CategoryId);
        }
    }
}
