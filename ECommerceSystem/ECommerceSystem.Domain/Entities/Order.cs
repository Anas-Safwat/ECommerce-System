using ECommerceSystem.Domain.Enums;
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
        public User User { get; set; } = null!;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
