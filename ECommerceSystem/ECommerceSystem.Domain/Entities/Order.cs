using ECommerceSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTimeOffset OrderDate { get; set; }

        //Navigation Properties
        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
