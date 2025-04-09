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
                roadMap.NewbieId = newRoadMap.NewbieId;
                roadMap.CreatorId = newRoadMap.CreatorId;
                roadMap.Title = newRoadMap.Title ?? "";
                roadMap.Description = newRoadMap.Description ?? "";
                roadMap.AssignDate = newRoadMap.AssignDate ?? DateTime.Now;
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
                }).ToListAsync();

            foreach (var roadMap in roadMaps)
            {
                int allPointsCount = await _context.RoadMapPoints
                    .Where(rmp => rmp.State == StateEnum.Active 
                        && rmp.RoadMapId == roadMap.Id)
                    .CountAsync();

                int finishedPointsCount = await _context.RoadMapPoints
                    .Where(rmp => rmp.State == StateEnum.Active
                        && rmp.RoadMapId == roadMap.Id
                        && _context.Tasks
                            .Where(t => t.State == StateEnum.Active && t.RoadMapPointId == rmp.Id)
                            .Any()
                        && _context.Tasks
                            .Where(t => t.State == StateEnum.Active 
                                && t.RoadMapPointId == rmp.Id)
                            .All(t => t.Status == StatusEnum.Done))
                    .CountAsync();

                if (finishedPointsCount == 0)
                {
                    roadMap.Status = StatusEnum.ToDo;
                }
                else if (allPointsCount == finishedPointsCount)
                {
                    roadMap.Status = StatusEnum.Done;
                }
                else
                {
                    roadMap.Status = StatusEnum.InProgress;
                    roadMap.Progress = Math.Round((decimal)finishedPointsCount / (decimal)allPointsCount * 100, 2);
                }

                roadMap.CreatorName = await _userRepository.GetUserNameByIdAsync(roadMap.CreatorId);
            }

            return roadMaps;
        }
    }
}
