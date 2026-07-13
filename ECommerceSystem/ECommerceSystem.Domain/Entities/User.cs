using ECommerceSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerceSystem.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public DateTimeOffset CreatedAt { get; set; }


        // Navigation Properties
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
