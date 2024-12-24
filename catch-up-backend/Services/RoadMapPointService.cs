using catch_up_backend.Controllers;
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

        public async Task<RoadMapPointDto> AddAsync(RoadMapPointDto newRoadMapPoint)
        {
            try
            {
                var roadMapPoint = new RoadMapPointModel(
                    newRoadMapPoint.RoadMapId,
                    newRoadMapPoint.Name ?? "",
                    newRoadMapPoint.Deadline);
                await _context.AddAsync(roadMapPoint);
                await _context.SaveChangesAsync();
                newRoadMapPoint.Id = roadMapPoint.Id;
            }
            catch (Exception e)
            {
                throw new Exception("Error: Road Map Add " + e);
            }
            return newRoadMapPoint;
        }

        public async Task<RoadMapPointDto> EditAsync(int roadMapPointId, RoadMapPointDto newRoadMapPoint)
        {
            var roadMapPoint = await _context.RoadMapPoints.FindAsync(roadMapPointId);
            if (roadMapPoint == null)
                return null;
            try
            {
                roadMapPoint.RoadMapId = newRoadMapPoint.RoadMapId;
                roadMapPoint.Name = newRoadMapPoint.Name ?? "";
                roadMapPoint.StartDate = newRoadMapPoint.StartDate ?? DateTime.Now;
                roadMapPoint.FinishDate = newRoadMapPoint.FinishDate;
                _context.RoadMapPoints.Update(roadMapPoint);
                await _context.SaveChangesAsync();
                newRoadMapPoint.Id = roadMapPoint.Id;
            }
            catch (Exception e)
            {
                throw new Exception("Error: Road Map Add " + e);
            }
            return newRoadMapPoint;
        }
        public async Task<bool> SetStatusAsync(int roadMapPointId, StatusEnum status)
        {
            var roadMapPoint = await _context.RoadMapPoints.FindAsync(roadMapPointId);
            if (roadMapPoint == null)
                return false;
            try
            {
                roadMapPoint.Status = status;
                if(status == StatusEnum.ToReview)
                    roadMapPoint.FinishDate = DateTime.Now;
                _context.RoadMapPoints.Update(roadMapPoint);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error: RoadMap Finish " + e);
            }
            return true;

        }
        public async Task<bool> DeleteAsync(int roadMapPointId)
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
                throw new Exception("Error: RoadMap Delete " + e);
            }
            return true;

        }

        public async Task<List<RoadMapPointDto>> GetByRoadMapIdAsync(int roadMapId)
        {
            var roadMapPoints = await _context.RoadMapPoints
                .Where(rmp => rmp.State == StateEnum.Active && rmp.RoadMapId == roadMapId)
                .Select(rmp => new RoadMapPointDto
                {
                    Id = rmp.Id,
                    RoadMapId = rmp.RoadMapId,
                    Name = rmp.Name,
                    StartDate = rmp.StartDate,
                    FinishDate = rmp.FinishDate,
                    Deadline = rmp.Deadline,
                    Status = rmp.Status
                }).ToListAsync();

            return roadMapPoints;
        }
    }
}
