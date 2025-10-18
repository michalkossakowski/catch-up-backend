using catch_up_backend.Dtos;
using catch_up_backend.Enums;

namespace catch_up_backend.Interfaces
{
    public interface IBadgeService
    {
        public Task<bool> Add(BadgeDto newBadge);
        public Task<bool> Edit(int badgeId, BadgeDto newBadge);
        public Task<bool> Delete(int badgeId);
        public Task<BadgeDto> GetById(int badgeId);
        public Task<List<BadgeDto>> GetAll();
        public Task<List<BadgeDto>> GetByMentorId(Guid userId);
        public Task AssignBadgeManuallyAsync(Guid userId, int badgeId);
        public Task HandleUserBadgesAsync(Guid userId, BadgeTypeCountEnum counterToIncrememt);
    }
}
