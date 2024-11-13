using catch_up_backend.Dtos;
using catch_up_backend.Models;
using catch_up_backend.Interfaces;
using catch_up_backend.Database;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Enums;

namespace catch_up_backend.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly CatchUpDbContext _context;

        public BadgeService(CatchUpDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Add(BadgeDto newBadge)
        {
            try
            {
                var badge = new BadgeModel(
                newBadge.Name ?? "",
                newBadge.Description ?? "",
                newBadge.IconSource ?? "",
                newBadge.Count,
                newBadge.CountType);
                await _context.AddAsync(badge);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw new Exception("Error: Add badge: " + ex);
            }
            return true;
        }
        public async Task<bool> Edit(int badgeId, BadgeDto newBadge)
        {
            var badge = await _context.Badges.FindAsync(badgeId);
            if (badge == null) {
                return false;
            }
            try {
                badge.Name = newBadge.Name;
                badge.Description = newBadge.Description;
                badge.IconSource = newBadge.IconSource;
                badge.Count = newBadge.Count;
                badge.CountType = newBadge.CountType;
                _context.Badges.Update(badge);
                await _context.SaveChangesAsync();
            } 
            catch (Exception ex) {
                throw new Exception("Error: Edit badge:" + ex);
            }
            return true;
        }
        public async Task<bool> Delete(int badgeId)
        {
            var badge = await _context.Badges.FindAsync(badgeId);
            if (badge == null)
            {
                return false;
            }
            try {
                badge.State = StateEnum.Deleted;
                await _context.SaveChangesAsync();
            } catch (Exception ex) {
                throw new Exception("Error: Delete badge:" + ex);
            }
            return true;
        }
        public async Task<BadgeDto> GetById(int badgeId)
        {
            var badge = await _context.Badges
                .Where(b => b.Id == badgeId && b.State != StateEnum.Deleted)
                .Select(b => new BadgeDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    IconSource = b.IconSource,
                    Count = b.Count,
                    CountType = b.CountType
                }).FirstOrDefaultAsync();

            return badge;
        }
        public async Task<List<BadgeDto>> GetAll()
        {
            var badges = await _context.Badges
                .Where(b => b.State != StateEnum.Deleted)
                .Select(b => new BadgeDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    IconSource = b.IconSource,
                    Count = b.Count,
                    CountType = b.CountType
                })
               .ToListAsync();

            return badges;
        }
    }
}
