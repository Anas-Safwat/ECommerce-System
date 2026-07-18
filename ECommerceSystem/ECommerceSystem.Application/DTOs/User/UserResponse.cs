using ECommerceSystem.Domain.Enums;

namespace ECommerceSystem.Application.DTOs.User
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
