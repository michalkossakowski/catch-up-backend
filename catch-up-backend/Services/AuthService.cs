﻿using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using catch_up_backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace catch_up_backend.Services
{
    public class AuthService : IAuthService{
        private readonly IConfiguration _config;
        private readonly IUserRepository userRepository;
        private readonly IRefreshTokenRepository refreshTokenRepository;

        public AuthService(IConfiguration config, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository){
            this._config = config;
            this.userRepository = userRepository;
            this.refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthResponseDto> Login(LoginRequestDto request)
        {
            var user = await userRepository.GetByMail(request.Email);

            if (user == null)
                return null;
            
            if (!VerifyPassword(request.Password, user.Password))
                return null;

            // Check if refresh token exists
            var storedToken = await refreshTokenRepository.GetByUserId(user.Id);
            if (storedToken != null){
                await refreshTokenRepository.Delete(storedToken);
            }

            var accessToken = GenerateAccessToken(user.Id, user.Type);
            var refreshToken = GenerateRefreshToken(user.Id, user.Type);

            await StoreRefreshToken(user.Id, refreshToken);

            return new AuthResponseDto(accessToken, refreshToken);
        }

        private bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(plainPassword);
                var hash = sha256.ComputeHash(bytes);
                var hashedInput = Convert.ToBase64String(hash);

                return hashedPassword == hashedInput;
            }
        }

        private string GenerateAccessToken(Guid id, string userType){
            var accessToken = GenerateJwtToken(
                id,
                userType,
                _config["Jwt:AccessTokenSecret"],
                TimeSpan.FromHours(12)
            );

            return accessToken;
        }

        private string GenerateRefreshToken(Guid id, string userType){
            var refreshToken = GenerateJwtToken(
                id,
                userType,
                _config["Jwt:AccessTokenSecret"],
                TimeSpan.FromDays(7)
            );

            return refreshToken;
        }

        private string GenerateJwtToken(Guid userId, string userType, string secret, TimeSpan whenExpire){
            var tokenHandler = new JwtSecurityTokenHandler(); // create a new JWT token handler
            var key = Encoding.ASCII.GetBytes(secret); // convert secret key into byte array
            // create a token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity([
                     new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // user ID
                     new Claim(ClaimTypes.Role, userType) // role
                ]),
                // set the issuer and audience claims from the app secret config
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                // set the token expiration time
                Expires = DateTime.UtcNow.Add(whenExpire),
                // set the signing credentials using the secret key and the HMAC-SHA256 algorithm
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            // create the token and write it to a string
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task StoreRefreshToken(Guid userId, string refreshToken){
            var refreshTokenModel = new RefreshTokenModel(userId, refreshToken, DateTime.UtcNow.AddDays(7));
            await refreshTokenRepository.Add(refreshTokenModel);
        }

        public async Task<AuthResponseDto> RefreshToken(string refreshToken){
            // Validate JWT format
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:AccessTokenSecret"]);

            var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters{   
                // Validate the issuer signing key, issuer, and audience
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            // Extract user ID from token
            var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Check if refresh token exists and is valid
            var storedToken = await refreshTokenRepository.DoesTokenExist(refreshToken, userId);
            if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Invalid or expired refresh token");

            // Get user and generate new tokens
            var user = await userRepository.GetById(userId);
            if (user == null)
                throw new Exception("User not found");

            var accessToken = GenerateAccessToken(user.Id, user.Type);
            var newRefreshToken = GenerateRefreshToken(user.Id, user.Type);

            // Replace old refresh token
            await refreshTokenRepository.Delete(storedToken);
            await StoreRefreshToken(userId, newRefreshToken);

            return new AuthResponseDto(accessToken, newRefreshToken);
        }
    }
}
