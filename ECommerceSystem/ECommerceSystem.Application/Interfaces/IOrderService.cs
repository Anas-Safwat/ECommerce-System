using ECommerceSystem.Application.DTOs.Order;

namespace ECommerceSystem.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetUserOrdersAsync(Guid userId);
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync(); // For Admin
        Task<OrderResponse?> GetOrderByIdAsync(int id, Guid userId, bool isAdmin = false);
        Task<OrderResponse?> CreateOrderAsync(Guid userId, CreateOrderRequest request);
        Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusRequest request);
    }
}
