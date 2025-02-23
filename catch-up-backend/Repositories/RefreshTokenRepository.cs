using catch_up_backend.Database;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly CatchUpDbContext _context;

        public RefreshTokenRepository(CatchUpDbContext context)
        {
            _context = context;
        }

        public async Task Add(RefreshTokenModel refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(RefreshTokenModel refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshTokenModel> GetByUserId(string refreshToken, Guid userId)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken && t.UserId == userId);
        }
        public async Task<Guid> GetUserIdByRefreshToken(string refreshToken)
        {
            var userRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            if (userRefreshToken == null)
            {
                throw new Exception($"User with this token: [{refreshToken}] not exists");
            }
            return userRefreshToken.UserId;
        }

        public async Task<string> GetRefreshTokenByUserId(Guid userId)
        {
            var userRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId);
            if (userRefreshToken == null)
            {
                throw new Exception($"Refresh token for user: [{userId}] not exists");
            }
            return userRefreshToken.Token;
        }
    }
}
