﻿using catch_up_backend.Dtos;
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
                newBadge.IconId,
                //newBadge.IconSource ?? "",
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
                //badge.IconSource = newBadge.IconSource;
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

        public async Task AssignBadgeManuallyAsync(Guid userId, int badgeId)
        {
            await ExecuteAssignment(userId, badgeId);
        }

        public async Task AssignBadgeAutomatically(Guid userId, BadgeTypeCountEnum countType, int count)
        {
            int? badgeId = await CheckConditions(countType, count);

            if (badgeId.HasValue)
            {
                await ExecuteAssignment(userId, badgeId.Value);
                Console.WriteLine($"Assign badge {badgeId.Value} to mentor {userId}.");
            }
            else
            {
                Console.WriteLine("No badge to assign.");
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

        private async Task ExecuteAssignment(Guid mentorId, int badgeId)
        {
            try
            {
                var mentorBadge = new MentorBadgeModel(mentorId, badgeId);

                await _context.MentorsBadges.AddAsync(mentorBadge);

                await _context.SaveChangesAsync();

                Console.WriteLine($"Assign badge {badgeId} to mentor {mentorId} with date: {mentorBadge.AchievedDate}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while assign badge: {ex.Message}");
            }
        }

        public async Task<List<MentorBadgeDto>> GetByMentorId(Guid userId)
        {
            var mentorBadges = await _context.MentorsBadges
                .Where(mb => mb.MentorId == userId)
                .Join(_context.Badges,
                    mb => mb.BadgeId,
                    b => b.Id,
                    (mb, b) => new { MentorBadge = mb, Badge = b })
                .Where(x => x.Badge.State != StateEnum.Deleted)
                .Select(x => new MentorBadgeDto
                {
                    MentorId = x.MentorBadge.MentorId,
                    BadgeId = x.MentorBadge.BadgeId,
                    AchievedDate = x.MentorBadge.AchievedDate
                })
                .ToListAsync();

            return mentorBadges;
        }

    }
}
