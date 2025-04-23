using catch_up_backend.Controllers;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Services
{
    public class RoadMapService : IRoadMapService
    {
        private readonly CatchUpDbContext _context;
        private readonly IUserRepository _userRepository;

        public RoadMapService(CatchUpDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<RoadMapDto> AddAsync(RoadMapDto newRoadMap)
        {
            try
            {
                var roadMap = new RoadMapModel(
                    newRoadMap.NewbieId,
                    newRoadMap.CreatorId,
                    newRoadMap.Title ?? "",
                    newRoadMap.Description ?? ""
                    );
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
                roadMap.Title = newRoadMap.Title ?? "";
                roadMap.Description = newRoadMap.Description ?? "";

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

        public async Task<bool> DeleteAsync(int roadMapId, bool deleteTasksInside = false)
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
                    Title = rm.Title,
                    Description = rm.Description,
                    NewbieId = rm.NewbieId,
                    CreatorId = rm.CreatorId,
                    AssignDate = rm.AssignDate,
                    FinishDate = rm.FinishDate,
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
                    Title = rm.Title,
                    Description = rm.Description,
                    NewbieId = rm.NewbieId,
                    CreatorId = rm.CreatorId,
                    AssignDate = rm.AssignDate,
                    FinishDate = rm.FinishDate,
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
                    Title = rm.Title,
                    Description = rm.Description,
                    NewbieId = rm.NewbieId,
                    CreatorId = rm.CreatorId,
                    AssignDate = rm.AssignDate,
                    FinishDate = rm.FinishDate,
                    Status = rm.Status,
                    Progress = rm.Progress
                }).ToListAsync();

            foreach (var roadMap in roadMaps)
            {
                roadMap.CreatorName = await _userRepository.GetUserNameByIdAsync(roadMap.CreatorId);
            }

            return roadMaps;
        }

        public async Task UpdateRoadMapStatus(int roadMapId)
        {
            var roadMap = await _context.RoadMaps.FirstOrDefaultAsync(rmp => rmp.Id == roadMapId);

            var activePoints = await _context.RoadMapPoints
                .Where(rmp => rmp.State == StateEnum.Active)
                .ToListAsync() 
                ?? new List<RoadMapPointModel>();

            var finishedPoints = activePoints
                .Where(rmp => rmp.Status == StatusEnum.Done)
                .ToList() 
                ?? new List<RoadMapPointModel>();

            roadMap.Progress = Math.Round((decimal)finishedPoints.Count / activePoints.Count * 100, 2);

            if (finishedPoints.Count == 0)
            {
                roadMap.Status = StatusEnum.ToDo;
            }
            else if (activePoints.All(t => t.Status == StatusEnum.Done))
            {
                roadMap.Status = StatusEnum.Done;
                roadMap.FinishDate = DateTime.Now;
            }
            else
            {
                roadMap.Status = StatusEnum.InProgress;
            }

            _context.RoadMaps.Update(roadMap);
            await _context.SaveChangesAsync();
        }
    }
}
