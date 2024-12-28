using catch_up_backend.Dtos;
using catch_up_backend.Enums;

namespace catch_up_backend.Interfaces
{
    public interface IRoadMapPointService
    {
        public Task<RoadMapPointDto> AddAsync(RoadMapPointDto newRoadMapPoint);
        public Task<RoadMapPointDto> EditAsync(int roadMapPointId, RoadMapPointDto newRoadMapPoint);
        public Task<bool> SetStatusAsync(int roadMapPointId, StatusEnum status);
        public Task<bool> DeleteAsync(int roadMapPointId);
        public Task<List<RoadMapPointDto>> GetByRoadMapIdAsync(int roadMapId);
    }
}
