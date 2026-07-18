using ECommerceSystem.Application.DTOs.Auth;
using ECommerceSystem.Domain.Entities;
using ECommerceSystem.Domain.Enums;

namespace ECommerceSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(string email, string password, UserRole role);
        Task<AuthResponse?> LoginAsync(string email, string password);
        Task<AuthResponse?> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(Guid userId, string refreshToken);
        Task LogoutAllAsync(Guid userId);
        string GenerateToken(User user);
    }
}
