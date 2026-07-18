using ECommerceSystem.Application.DTOs.Cart;

namespace ECommerceSystem.Application.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemResponse>> GetCartItemsAsync(Guid userId);
        Task<CartItemResponse?> AddCartItemAsync(Guid userId, AddCartItemRequest request);
        Task<bool> UpdateCartItemAsync(int id, Guid userId, UpdateCartItemRequest request);
        Task<bool> RemoveCartItemAsync(int id, Guid userId);
        Task<bool> ClearCartAsync(Guid userId);
    }
}
