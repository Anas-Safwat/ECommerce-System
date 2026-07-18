using ECommerceSystem.Domain.Enums;

namespace ECommerceSystem.Application.DTOs.Auth
{
    public class RegisterRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public UserRole Role { get; set; }
    }
}
