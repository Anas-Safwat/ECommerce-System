using ECommerceSystem.Application.DTOs.Auth;
using ECommerceSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerceSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request.Email, request.Password, request.Role);

            if (!result)
                return Conflict(AuthResponse.FailureResponse("A user with this email already exists."));

            return Ok(AuthResponse.RegistrationSuccess());
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request.Email, request.Password);

            if (response == null)
                return Unauthorized(AuthResponse.FailureResponse("Invalid email or password."));

            return Ok(response);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshTokenRequest request)
        {
            var response = await _authService.RefreshTokenAsync(request.RefreshToken);

            if (response == null)
                return Unauthorized(AuthResponse.FailureResponse("Invalid or expired refresh token."));

            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<AuthResponse>> Logout([FromBody] LogoutRequest request)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized(AuthResponse.FailureResponse("Invalid token."));

            var result = await _authService.LogoutAsync(userId.Value, request.RefreshToken);

            if (!result)
                return BadRequest(AuthResponse.FailureResponse("Refresh token not found or already revoked."));

            return Ok(AuthResponse.RegistrationSuccess("Logged out successfully."));
        }

        [HttpPost("logout-all")]
        [Authorize]
        public async Task<ActionResult<AuthResponse>> LogoutAll()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized(AuthResponse.FailureResponse("Invalid token."));

            await _authService.LogoutAllAsync(userId.Value);

            return Ok(AuthResponse.RegistrationSuccess("All sessions have been logged out."));
        }

        private Guid? GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)
                           ?? User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                return null;

            return userId;
        }
    }
}
