using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using catch_up_backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        public async Task<AuthResponseDto> Login(LoginRequestDto request){
            var user = await userRepository.GetByMail(request.Email);

            if (user == null)
                throw new Exception("No such user exists");
            if (user.Password != request.Password){
                throw new Exception("Wrong password");
            }

            var (accesToken, refreshToken) = GenerateTokens(user);
            await StoreRefreshToken(user.Id, refreshToken);

            return new AuthResponseDto(accesToken, refreshToken);
        }

        public async Task<AuthResponseDto> Register(RegisterRequestDto request){
            var existingUser = await userRepository.GetByMail(request.Email);
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

            await userRepository.Add(newUser);
            var user = await userRepository.GetByMail(request.Email);

            var (accessToken, refreshToken) = GenerateTokens(user);
            await StoreRefreshToken(user.Id, refreshToken);

            return new AuthResponseDto(accessToken, refreshToken);
        }

        private (string accessToken, string refreshToken) GenerateTokens(UserModel user){
            var accessToken = GenerateJwtToken(
                user.Id,
                _config["Jwt:AccessTokenSecret"],
                TimeSpan.FromSeconds(30)
            );

            var refreshToken = GenerateJwtToken(
                user.Id,
                _config["Jwt:AccessTokenSecret"],
                TimeSpan.FromDays(7)
            );

            return (accessToken, refreshToken);
        }

        private string GenerateJwtToken(Guid userId, string secret, TimeSpan whenExpire){
            var tokenHandler = new JwtSecurityTokenHandler(); // create a new JWT token handler
            var key = Encoding.ASCII.GetBytes(secret); // convert secret key into byte array
            // create a token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, userId.ToString())]), // set the subject of the token to be the user ID
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
            var storedToken = await refreshTokenRepository.GetByUserId(refreshToken, userId);
            if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Invalid or expired refresh token");

            // Get user and generate new tokens
            var user = await userRepository.GetById(userId);
            if (user == null)
                throw new Exception("User not found");
            var (accessToken, newRefreshToken) = GenerateTokens(user);

            // Replace old refresh token
            await refreshTokenRepository.Delete(storedToken);
            await StoreRefreshToken(userId, newRefreshToken);

            return new AuthResponseDto(accessToken, newRefreshToken);
        }
    }
}
