using ECommerceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ECommerceSystem.Infrastructure.Configurations
{
    internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // 1) Table and key
            builder.ToTable("OrderItems");
            builder.HasKey(o => o.Id);

            //2) Property Constraints
            builder.Property(o => o.Quantity)
                .IsRequired();

            builder.Property(o => o.UnitPrice)
                .IsRequired()
                .HasPrecision(18, 2);
               
        }
    }
}
