namespace ECommerceSystem.Application.DTOs.Auth
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTimeOffset? RefreshTokenExpiresAt { get; set; }
        public string? Message { get; set; }

        public static AuthResponse SuccessResponse(string token, string refreshToken, DateTimeOffset refreshTokenExpiresAt, string message = "Authentication successful.")
        {
            return new AuthResponse
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = refreshTokenExpiresAt,
                Message = message
            };
        }

        public static AuthResponse RegistrationSuccess(string message = "Registration successful.")
        {
            return new AuthResponse
            {
                Success = true,
                Message = message
            };
        }

        public static AuthResponse FailureResponse(string message)
        {
            return new AuthResponse
            {
                Success = false,
                Message = message
            };
        }
    }
}
