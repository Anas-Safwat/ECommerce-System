using ECommerceSystem.Domain.Enums;

namespace ECommerceSystem.Application.DTOs.Order
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusDisplay { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; }
        public Guid UserId { get; set; }
        public List<OrderItemResponse> Items { get; set; } = new();
    }
}
