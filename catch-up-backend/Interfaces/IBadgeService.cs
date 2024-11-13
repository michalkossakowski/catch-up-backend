using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface IBadgeService
    {
        public Task<bool> Add(BadgeDto newBadge);
        public Task<bool> Edit(int badgeId, BadgeDto newBadge);
        public Task<bool> Delete(int badgeId);
        public Task<BadgeDto> GetById(int badgeId);
        public Task<List<BadgeDto>> GetAll();
    }
}
