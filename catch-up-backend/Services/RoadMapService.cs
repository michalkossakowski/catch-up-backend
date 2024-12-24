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

        public async Task<RoadMapDto> AddAsync(RoadMapDto newRoadMap)
        {
            try
            {
                var roadMap = new RoadMapModel(
                    newRoadMap.NewbieId,
                    newRoadMap.Name ?? "");
                await _context.AddAsync(roadMap);
                await _context.SaveChangesAsync();
                newRoadMap.Id = roadMap.Id;
            }
            catch (Exception e)
            {
                throw new Exception("Error: Road Map Add " + e);
            }
            return newRoadMap;
        }

        public async Task<RoadMapDto> EditAsync(int roadMapId, RoadMapDto newRoadMap)
        {
            var roadMap = await _context.RoadMaps.FindAsync(roadMapId);
            if (roadMap == null)
                return null;
            try
            {
                roadMap.NewbieId = newRoadMap.NewbieId;
                roadMap.Name = newRoadMap.Name ?? "";
                roadMap.StartDate = newRoadMap.StartDate ?? DateTime.Now;
                roadMap.FinishDate = newRoadMap.FinishDate;
                _context.RoadMaps.Update(roadMap);
                await _context.SaveChangesAsync();
                newRoadMap.Id = roadMap.Id;
            }
            catch (Exception e)
            {
                throw new Exception("Error: Road Map Add " + e);
            }
            return newRoadMap;
        }
        public async Task<bool> SetStatusAsync(int roadMapId, StatusEnum status)
        {
            var roadMap = await _context.RoadMaps.FindAsync(roadMapId);
            if (roadMap == null)
                return false;
            try
            {
                roadMap.Status = status;
                if(status == StatusEnum.Done)
                    roadMap.FinishDate = DateTime.Now;

                _context.RoadMaps.Update(roadMap);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error: RoadMap SetStatus " + e);
            }
            return true;
        }
        public async Task<bool> DeleteAsync(int roadMapId)
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

        public async Task<List<RoadMapDto>> GetAllAsync()
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
                    Status = rm.Status,
                })
               .ToListAsync();

            return roadMaps;
        }

        public async Task<RoadMapDto> GetByIdAsync(int roadMapId)
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
                    Status = rm.Status,
                }).FirstOrDefaultAsync();

            return roadMap;
        }

        public async Task<List<RoadMapDto>> GetByNewbieIdAsync(Guid newbieId)
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
                    Status = rm.Status,
                }).ToListAsync();

            return roadMaps;
        }
    }
}
