using ECommerceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Infrastructure.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Table and PK
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);

            //Properties
            builder.Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            builder.Property(o => o.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(o => o.OrderDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            //Relationships
            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
