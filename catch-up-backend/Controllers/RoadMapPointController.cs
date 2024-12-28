using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoadMapPoint : ControllerBase
    {
        private readonly IRoadMapPointService _roadMapPointService;
        public RoadMapPoint(IRoadMapPointService roadMapPointService) 
        {
            _roadMapPointService = roadMapPointService;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] RoadMapPointDto roadMapPoint)
        {
            return await _roadMapPointService.Add(roadMapPoint) 
                ? Ok(new { message = "RoadMapPoint added", roadMap = roadMapPoint })
                : StatusCode(500, new { message = "RoadMapPoint adding error" });
        }

        [HttpPut]
        [Route("SetStatus/{roadMapPointId:int}/{status:int}")]
        public async Task<IActionResult> SetStatus(int roadMapPointId, int status)
        {
            return await _roadMapPointService.SetStatus(roadMapPointId, (StatusEnum)status)
                ? Ok(new { message = "RoadMapPoint status changed", roadMap = roadMapPointId, status = status })
                : StatusCode(500, new { message = "RoadMapPoint setting status error", roadMap = roadMapPointId });
        }

        [HttpDelete]
        [Route("Delete/{roadMapPointId:int}")]
        public async Task<IActionResult> Delete(int roadMapPointId)
        {
            return await _roadMapPointService.Delete(roadMapPointId) 
                ? Ok(new { message = "RoadMapPoint deleted", roadMap = roadMapPointId })
                : NotFound(new { message = "RoadMapPoint not found", roadMap = roadMapPointId });
        }


        [HttpGet]
        [Route("GetByRoadMapId/{roadMapId:int}")]
        public async Task<IActionResult> GetByRoadMapId(int roadMapId)
        {
            var roadMapPoints = await _roadMapPointService.GetByRoadMapId(roadMapId);
            if (!roadMapPoints.Any())
                return NotFound(new { message = $"There is no any RoadMapPoints for this RoadMap: [{roadMapId}]" });
            return Ok(roadMapPoints);
        }
    }
}
