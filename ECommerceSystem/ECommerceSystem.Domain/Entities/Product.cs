using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool Stock { get; set; }
        public DateTimeOffset CreatedAt { get; set; }


        // Navigation Properties:

        public Guid SellerId { get; set; }
        public User Seller { get; set; }

        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
