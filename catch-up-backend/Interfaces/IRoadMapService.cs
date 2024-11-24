using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface IRoadMapService
    {
        public Task<bool> Add(RoadMapDto newRoadMap);
        public Task<bool> Finish(int roadMapId);
        public Task<bool> Delete(int roadMapId);
        public Task<List<RoadMapDto>> GetAll();
        public Task<RoadMapDto> GetById(int roadMapId);
        public Task<List<RoadMapDto>> GetByNewbieId(Guid newbieId);
    }
}
