namespace ECommerceSystem.Application.DTOs.Order
{
    public class CreateOrderRequest
    {
        public List<CreateOrderItemRequest> Items { get; set; } = new();
    }
}
