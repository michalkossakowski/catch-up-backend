using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Services
{
    public class RoadMapPointService : IRoadMapPointService
    {
        private readonly CatchUpDbContext _context;

        public RoadMapPointService(CatchUpDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(RoadMapPointDto newRoadMapPoint)
        {
            try
            {
                var roadMapPoint = new RoadMapPointModel(
                    newRoadMapPoint.RoadMapId,
                    newRoadMapPoint.Name ?? "",
                    newRoadMapPoint.Deadline ?? 0);

                await _context.AddAsync(roadMapPoint);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error: Road Map AddAsync " + e);
            }
            return true;


        }
        public async Task<bool> SetStatus(int roadMapPointId, StatusEnum status)
        {
            var roadMapPoint = await _context.RoadMapPoints.FindAsync(roadMapPointId);
            if (roadMapPoint == null)
                return false;
            try
            {
                roadMapPoint.Status = status;
                if(status == StatusEnum.ToReview)
                    roadMapPoint.FinalizationDate = DateTime.Now;
                _context.RoadMapPoints.Update(roadMapPoint);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error: RoadMap Finish " + e);
            }
            return true;

        }
        public async Task<bool> Delete(int roadMapPointId)
        {
            var roadMapPoint = await _context.RoadMapPoints.FindAsync(roadMapPointId);
            if (roadMapPoint == null)
                return false;
            try
            {
                roadMapPoint.State = StateEnum.Deleted;
                _context.RoadMapPoints.Update(roadMapPoint);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error: RoadMap DeleteAsync " + e);
            }
            return true;

        }

        public async Task<List<RoadMapPointDto>> GetByRoadMapId(int roadMapId)
        {
            var roadMapPoints = await _context.RoadMapPoints
                .Where(rmp => rmp.State == StateEnum.Active && rmp.RoadMapId == roadMapId)
                .Select(rmp => new RoadMapPointDto
                {
                    Id = rmp.Id,
                    RoadMapId = rmp.RoadMapId,
                    Name = rmp.Name,
                    AssignmentDate = rmp.AssignmentDate,
                    FinalizationDate = rmp.FinalizationDate,
                    Deadline = rmp.Deadline,
                    Status = rmp.Status
                }).ToListAsync();

            return roadMapPoints;
        }
    }
}
