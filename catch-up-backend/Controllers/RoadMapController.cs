using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Helpers;
using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoadMap : ControllerBase
    {
        private readonly IRoadMapService _roadMapService;
        public RoadMap(IRoadMapService roadMapService) 
        {
            _roadMapService = roadMapService;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] RoadMapDto newRoadMap)
        {
            var creatorId = TokenHelper.GetUserIdFromTokenInRequest(Request);
            newRoadMap.CreatorId = creatorId;
            var result = await _roadMapService.AddAsync(newRoadMap);
            return result != null
                ? Ok(new { message = "RoadMap added", roadMap = result })
                : StatusCode(500, new { message = "RoadMap adding error" });
        }

        [HttpPut]
        [Route("Edit/{roadMapId:int}")]
        public async Task<IActionResult> Edit(int roadMapId, [FromBody] RoadMapDto newRoadMap)
        {
            var result = await _roadMapService.EditAsync(roadMapId, newRoadMap);
            return result != null
                ? Ok(new { message = "RoadMap added", roadMap = result })
                : StatusCode(500, new { message = "RoadMap adding error" });
        }

        [HttpDelete]
        [Route("Delete/{roadMapId:int}")]
        public async Task<IActionResult> Delete(int roadMapId)
        {
            return await _roadMapService.DeleteAsync(roadMapId) 
                ? Ok(new { message = "RoadMap deleted", roadMap = roadMapId })
                : NotFound(new { message = "RoadMap not found", roadMap = roadMapId });
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var roadMaps = await _roadMapService.GetAllAsync();
            if (!roadMaps.Any())
                return Ok(new { message = "No RoadMaps found" });
            return Ok(roadMaps);
        }

        [HttpGet]
        [Route("GetByNewbieId/{newbieId:guid}")]
        public async Task<IActionResult> GetByNewbieId(Guid newbieId)
        {
            var roadMaps = await _roadMapService.GetByNewbieIdAsync(newbieId);
            if (!roadMaps.Any())
                return NotFound(new { message = $"There is no any RoadMaps for Newbie: '{newbieId}'" });
            return Ok(roadMaps);
        }

        [HttpGet]
        [Route("GetMyRoadMaps")]
        public async Task<IActionResult> GetMyRoadmaps()
        {
            var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);

            var roadMaps = await _roadMapService.GetByNewbieIdAsync(userId);
            if (!roadMaps.Any())
                return Ok(new List<RoadMapDto>());
            return Ok(roadMaps);
        }
    }
}
