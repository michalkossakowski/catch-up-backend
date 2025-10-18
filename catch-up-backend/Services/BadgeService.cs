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
        private readonly INotificationService _notificationService;

        public BadgeService(
            CatchUpDbContext context,
            INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }
        public async Task<bool> Add(BadgeDto newBadge)
        {
            try
            {
                var badge = new BadgeModel(
                newBadge.Name ?? "",
                newBadge.Description ?? "",
                newBadge.IconId,
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
                badge.IconId = newBadge.IconId;
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
                    IconId = b.IconId,
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
                    IconId = b.IconId,
                    Count = b.Count,
                    CountType = b.CountType
                })
               .ToListAsync();

            return badges;
        }

        public async Task<List<BadgeDto>> GetByMentorId(Guid userId)
        {
            var mentorBadges = await _context.MentorsBadges
                .Where(mb => mb.MentorId == userId)
                .Join(_context.Badges,
                    mb => mb.BadgeId,
                    b => b.Id,
                    (mb, b) => new { MentorBadge = mb, Badge = b })
                .Where(x => x.Badge.State != StateEnum.Deleted)
                .Select(x => new BadgeDto
                {
                    Id = x.Badge.Id,
                    Name = x.Badge.Name,
                    Description = x.Badge.Description,
                    IconId = x.Badge.IconId,
                    Count = x.Badge.Count,
                    CountType = x.Badge.CountType,
                    AchievedDate = x.MentorBadge.AchievedDate
                })
                .ToListAsync();

            return mentorBadges;
        }

        public async Task AssignBadgeManuallyAsync(Guid userId, int badgeId)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(b => b.Id == badgeId);

            if(badge == null)
            {
                throw new ArgumentOutOfRangeException($"Badge with id {badgeId} not found.");
            }

            await AssignBadgeAsync(userId, badge);
        }

        public async Task HandleUserBadgesAsync(Guid userId, BadgeTypeCountEnum counterToIncrement)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentException($"User with id {userId} not found.");
            }

            if (user.Counters == null)
            {
                user.Counters = new Dictionary<BadgeTypeCountEnum, int>();
            }
            
            user.Counters[counterToIncrement] = user.Counters.GetValueOrDefault(counterToIncrement, 0) + 1;

            _context.Update(user);
            await _context.SaveChangesAsync();

            var userBadges = await _context.MentorsBadges
                .Where(mb => mb.MentorId == userId)
                .Select(mb => mb.BadgeId)
                .ToListAsync();

            var possibleBadges = await _context.Badges
                .Where(b => b.CountType == counterToIncrement
                    && b.State != StateEnum.Deleted
                    && !userBadges.Contains(b.Id))
                .ToListAsync();

            foreach (var badge in possibleBadges)
            {
                if(user.Counters[counterToIncrement] >= badge.Count)
                {
                    await AssignBadgeAsync(userId, badge);
                }
            }
        }

        public async Task<int?> CheckConditions(BadgeTypeCountEnum countType, int countToCheck)
        {
            var badge = await _context.Badges
                .Where(b => b.CountType == countType && b.State != StateEnum.Deleted && b.Count <= countToCheck)
                .OrderByDescending(b => b.Count)
                .FirstOrDefaultAsync();

            return badge?.Id;
        }

        private async Task AssignBadgeAsync(Guid mentorId, BadgeModel badge)
        {
            try
            {
                var mentorBadge = new MentorBadgeModel(mentorId, badge.Id);

                await _context.MentorsBadges.AddAsync(mentorBadge);

                await _context.SaveChangesAsync();

                await PrepareAndSendNotificationAsync(mentorId, badge);

                Console.WriteLine($"Assign badge {badge.Id} to mentor {mentorId} with date: {mentorBadge.AchievedDate}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while assign badge: {ex.Message}");
            }
        }

        private async Task PrepareAndSendNotificationAsync(Guid mentorId, BadgeModel badge)
        {
            var notification = new NotificationModel(
                mentorId,
                "New Badge Achieved!",
                $"Congratulations! You have earned a new badge: {badge.Name} !",
                $"/badges"
            );

            await _notificationService.AddNotification(notification, mentorId);
        }
    }
}
