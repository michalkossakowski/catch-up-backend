using catch_up_backend.Dtos;
using catch_up_backend.Enums;

namespace catch_up_backend.Interfaces
{
    public interface IRoadMapPointService
    {
        public Task<bool> Add(RoadMapPointDto newRoadMapPoint);
        public Task<bool> SetStatus(int roadMapPointId, StatusEnum status);
        public Task<bool> Delete(int roadMapPointId);
        public Task<List<RoadMapPointDto>> GetByRoadMapId(int roadMapId);
    }
}
