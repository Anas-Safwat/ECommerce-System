using ECommerceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceSystem.Infrastructure.Configurations
{
    internal class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            // 1) Table and key
            builder.ToTable("CartItems");
            builder.HasKey(c => c.Id);

            //2) Property Constraints
            builder.Property(c => c.Quantity)
                .IsRequired();
        }
    }
}
