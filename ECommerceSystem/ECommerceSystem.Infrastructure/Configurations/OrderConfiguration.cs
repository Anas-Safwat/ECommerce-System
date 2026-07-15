using ECommerceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ECommerceSystem.Infrastructure.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // 1) Table and key
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);

            //2) Property Constraints
            builder.Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            builder.Property(o => o.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(o => o.OrderDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            //3) Navigation and On Deleting
            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
