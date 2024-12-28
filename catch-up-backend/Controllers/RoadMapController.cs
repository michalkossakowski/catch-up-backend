using catch_up_backend.Dtos;
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
        [Route("AddAsync")]
        public async Task<IActionResult> Add([FromBody] RoadMapDto roadMap)
        {
            return await _roadMapService.Add(roadMap) 
                ? Ok(new { message = "RoadMap added", roadMap = roadMap })
                : StatusCode(500, new { message = "RoadMap adding error" });
        }

        [HttpPut]
        [Route("Finish/{roadMapId:int}")]
        public async Task<IActionResult> Finish(int roadMapId)
        {
            return await _roadMapService.Finish(roadMapId)
                ? Ok(new { message = "RoadMap finished", roadMap = roadMapId })
                : StatusCode(500, new { message = "RoadMap editing error", roadMap = roadMapId });
        }

        [HttpDelete]
        [Route("DeleteAsync/{roadMapId:int}")]
        public async Task<IActionResult> Delete(int roadMapId)
        {
            return await _roadMapService.Delete(roadMapId) 
                ? Ok(new { message = "RoadMap deleted", roadMap = roadMapId })
                : NotFound(new { message = "RoadMap not found", roadMap = roadMapId });
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var roadMaps = await _roadMapService.GetAll();
            if (!roadMaps.Any())
                return NotFound(new { message = "No RoadMaps found" });
            return Ok(roadMaps);
        }

        [HttpGet]
        [Route("GetByNewbieId/{newbieId:guid}")]
        public async Task<IActionResult> GetByNewbieId(Guid newbieId)
        {
            var roadMaps = await _roadMapService.GetByNewbieId(newbieId);
            if (!roadMaps.Any())
                return NotFound(new { message = $"There is no any RoadMaps for: [{newbieId}]" });
            return Ok(roadMaps);
        }
    }
}
