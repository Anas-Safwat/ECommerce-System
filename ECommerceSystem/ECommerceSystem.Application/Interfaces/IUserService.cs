using ECommerceSystem.Application.DTOs.User;

namespace ECommerceSystem.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        Task<UserResponse?> GetUserByIdAsync(Guid id);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
