using ECommerceSystem.Domain.Enums;

namespace ECommerceSystem.Application.DTOs.Order
{
    public class UpdateOrderStatusRequest
    {
        public OrderStatus Status { get; set; }
    }
}
