using Azure.Core;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace catch_up_backend.Services
{
    public class AuthService : IAuthService{
        private readonly CatchUpDbContext _context;
        private readonly IConfiguration _config;
        private readonly IUserService _userService;

        public AuthService(CatchUpDbContext context, IConfiguration config, IUserService userService){
            _context = context;
            _config = config;
            _userService = userService;
        }

        public async Task<AuthResponseDto> Login(LoginRequestDto request){
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if(user == null)
                throw new Exception("No such user exists");
            if (user.Password != request.Password){
                throw new Exception("Wrong password");
            }

            var (accesToken, refreshToken) = GenerateTokens(user);
            await StoreRefreshToken(user.Id, refreshToken);

            return new AuthResponseDto(accesToken, refreshToken);
        }

        public async Task<AuthResponseDto> Register(RegisterRequestDto request){
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (existingUser != null){
                throw new Exception("User with this email already exists");
            }

            var newUser = new UserDto{
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                Password = request.Password,
                Type = request.Type,
                Position = request.Position
            };

            await _userService.Add(newUser);
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            var (accessToken, refreshToken) = GenerateTokens(user);
            await StoreRefreshToken(user.Id, refreshToken);

            return new AuthResponseDto(accessToken, refreshToken);
        }

        private (string accessToken, string refreshToken) GenerateTokens(UserModel user){
            var accessToken = GenerateJwtToken(
                user.Id,
                _config["Jwt:AccessTokenSecret"],
                TimeSpan.FromMinutes(2)
            );

            var refreshToken = GenerateJwtToken(
                user.Id,
                _config["Jwt:AccessTokenSecret"],
                TimeSpan.FromDays(7)
            );

            return (accessToken, refreshToken);
        }

        private string GenerateJwtToken(Guid userId, string secret, TimeSpan whenExpire){
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            // crazy stuff down there
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, userId.ToString())]),

                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],

                Expires = DateTime.UtcNow.Add(whenExpire),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task StoreRefreshToken(Guid userId, string refreshToken){
            var refreshTokenModel = new RefreshTokenModel(userId, refreshToken, DateTime.UtcNow.AddDays(7));
            
            await _context.RefreshTokens.AddAsync(refreshTokenModel);
            await _context.SaveChangesAsync();
        }

        public async Task<AuthResponseDto> RefreshToken(string refreshToken){
            // Validate JWT format
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:AccessTokenSecret"]);

            var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,                           // Change to true
                ValidIssuer = _config["Jwt:Issuer"],            // Add this
                ValidateAudience = true,                         // Change to true
                ValidAudience = _config["Jwt:Audience"],         // Add this
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            // Extract user ID from token
            var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Check if refresh token exists and is valid
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken && t.UserId == userId);
            if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Invalid or expired refresh token");

            // Get user and generate new tokens
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found");
            var (accessToken, newRefreshToken) = GenerateTokens(user);

            // Replace old refresh token
            _context.RefreshTokens.Remove(storedToken);
            await StoreRefreshToken(userId, newRefreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponseDto(accessToken, newRefreshToken);
        }
    }
}
