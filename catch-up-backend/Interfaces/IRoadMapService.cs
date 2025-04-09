using catch_up_backend.Dtos;
using catch_up_backend.Enums;

namespace catch_up_backend.Interfaces
{
    public interface IRoadMapService
    {
        public Task<RoadMapDto> AddAsync(RoadMapDto newRoadMap);
        public Task<RoadMapDto> EditAsync(int roadMapId, RoadMapDto newRoadMap);
        public Task<bool> DeleteAsync(int roadMapId);
        public Task<List<RoadMapDto>> GetAllAsync();
        public Task<RoadMapDto> GetByIdAsync(int roadMapId);
        public Task<List<RoadMapDto>> GetByNewbieIdAsync(Guid newbieId);
    }
}
