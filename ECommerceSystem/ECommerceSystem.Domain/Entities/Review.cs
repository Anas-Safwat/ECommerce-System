using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public short Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }

        //Navigation Properties
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        
        

    }
}
