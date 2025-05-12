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

        public async Task<RefreshTokenModel> GetByUserId(Guid userId)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);
        }

        public async Task<RefreshTokenModel> DoesTokenExist(string refreshToken, Guid userId)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken && t.UserId == userId);
        }
    }
}
