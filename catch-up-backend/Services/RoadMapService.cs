using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Services
{
    public class RoadMapService : IRoadMapService
    {
        private readonly CatchUpDbContext _context;

        public RoadMapService(CatchUpDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(RoadMapDto newRoadMap)
        {
            try
            {
                var roadMap = new RoadMapModel(
                    newRoadMap.Name ?? "",
                    newRoadMap.NewbieId);
                await _context.AddAsync(roadMap);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error: Road Map Add " + e);
            }
            return true;


        }
        public async Task<bool> Finish(int roadMapId)
        {
            var roadMap = await _context.RoadMaps.FindAsync(roadMapId);
            if (roadMap == null)
                return false;
            try
            {
                roadMap.IsFinished = true;
                roadMap.FinishDate = DateTime.Now;
                _context.RoadMaps.Update(roadMap);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error: RoadMap Finish " + e);
            }
            return true;

        }
        public async Task<bool> Delete(int roadMapId)
        {
            var roadMap = await _context.RoadMaps.FindAsync(roadMapId);
            if (roadMap == null)
                return false;
            try
            {
                roadMap.State = StateEnum.Deleted;
                _context.RoadMaps.Update(roadMap);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error: RoadMap Delete " + e);
            }
            return true;

        }

        public async Task<List<RoadMapDto>> GetAll()
        {
            var roadMaps = await _context.RoadMaps
                .Where(rm => rm.State == StateEnum.Active)
                .Select(rm => new RoadMapDto
                {
                    Id = rm.Id,
                    Name = rm.Name,
                    NewbieId = rm.NewbieId,
                    StartDate = rm.StartDate,
                    FinishDate = rm.FinishDate,
                    IsFinished = rm.IsFinished
                })
               .ToListAsync();

            return roadMaps;
        }

        public async Task<RoadMapDto> GetById(int roadMapId)
        {
            var roadMap = await _context.RoadMaps
                .Where(rm => rm.State == StateEnum.Active && rm.Id == roadMapId)
                .Select(rm => new RoadMapDto
                {
                    Id = rm.Id,
                    Name = rm.Name,
                    NewbieId = rm.NewbieId,
                    StartDate = rm.StartDate,
                    FinishDate = rm.FinishDate,
                    IsFinished = rm.IsFinished
                }).FirstOrDefaultAsync();

            return roadMap;
        }

        public async Task<List<RoadMapDto>> GetByNewbieId(Guid newbieId)
        {
            var roadMaps = await _context.RoadMaps
                .Where(rm => rm.State == StateEnum.Active && rm.NewbieId == newbieId)
                .Select(rm => new RoadMapDto
                {
                    Id = rm.Id,
                    Name = rm.Name,
                    NewbieId = rm.NewbieId,
                    StartDate = rm.StartDate,
                    FinishDate = rm.FinishDate,
                    IsFinished = rm.IsFinished
                }).ToListAsync();

            return roadMaps;
        }
    }
}
