using BCrypt.Net;
using ECommerceSystem.Application.DTOs.Auth;
using ECommerceSystem.Application.Interfaces;
using ECommerceSystem.Domain.Entities;
using ECommerceSystem.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerceSystem.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<bool> RegisterAsync(string email, string password, UserRole role)
        {
            var existingUser = await _unitOfWork.Users.FindAsync(user => user.Email == email);
            if (existingUser != null)
                return false;

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = passwordHash,
                Role = role,
                CreatedAt = DateTimeOffset.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<AuthResponse?> LoginAsync(string email, string password)
        {
            var user = await _unitOfWork.Users.FindAsync(user => user.Email == email);
            if (user == null)
                return null;

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isPasswordValid)
                return null;

            string accessToken = GenerateToken(user);
            var refreshToken = await CreateRefreshTokenAsync(user.Id);

            return AuthResponse.SuccessResponse(accessToken, refreshToken.Token, refreshToken.ExpiresAt);
        }

        public async Task<AuthResponse?> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _unitOfWork.RefreshTokens.FindAsync(rt => rt.Token == refreshToken);
            if (storedToken == null || !storedToken.IsActive)
                return null;

            // Revoke the old refresh token (rotation)
            storedToken.RevokedAt = DateTimeOffset.UtcNow;
            _unitOfWork.RefreshTokens.Update(storedToken);

            // Get the user to generate a new access token
            var user = await _unitOfWork.Users.GetByIdAsync(storedToken.UserId);
            if (user == null)
                return null;

            // Issue new token pair
            string newAccessToken = GenerateToken(user);
            var newRefreshToken = await CreateRefreshTokenAsync(user.Id);

            return AuthResponse.SuccessResponse(newAccessToken, newRefreshToken.Token, newRefreshToken.ExpiresAt);
        }

        public async Task<bool> LogoutAsync(Guid userId, string refreshToken)
        {
            var storedToken = await _unitOfWork.RefreshTokens.FindAsync(
                rt => rt.Token == refreshToken && rt.UserId == userId);

            if (storedToken == null || !storedToken.IsActive)
                return false;

            storedToken.RevokedAt = DateTimeOffset.UtcNow;
            _unitOfWork.RefreshTokens.Update(storedToken);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task LogoutAllAsync(Guid userId)
        {
            var activeTokens = await _unitOfWork.RefreshTokens.FindAllAsync(
                rt => rt.UserId == userId && rt.RevokedAt == null);

            foreach (var token in activeTokens)
            {
                token.RevokedAt = DateTimeOffset.UtcNow;
                _unitOfWork.RefreshTokens.Update(token);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
           {
               new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
               new Claim(JwtRegisteredClaimNames.Email, user.Email),
               new Claim(ClaimTypes.Role, user.Role.ToString()),
               new Claim(JwtRegisteredClaimNames.Iat,
               DateTimeOffset.UtcNow.ToString(),
               ClaimValueTypes.Integer64
               )
           };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpirationInMinutes"]!)),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<RefreshToken> CreateRefreshTokenAsync(Guid userId)
        {
            var expirationDays = double.Parse(_configuration["Jwt:RefreshTokenExpirationInDays"] ?? "7");

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = userId,
                CreatedAt = DateTimeOffset.UtcNow,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(expirationDays)
            };

            await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return refreshToken;
        }
    }
}
