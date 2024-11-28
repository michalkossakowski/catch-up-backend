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
        public Task AssignBadgeManuallyAsync(Guid userId, int badgeId);
        public Task AssignBadgeAutomatically(Guid userId, BadgeTypeCountEnum countType, int count);
        public Task<int?> CheckConditions(string countType, int countToCheck);

    }
}
