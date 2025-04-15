﻿using catch_up_backend.Dtos;
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
        public async Task<IActionResult> AddAsync([FromBody] RoadMapPointDto roadMapPoint)
        {
            var result = await _roadMapPointService.AddAsync(roadMapPoint);
            return result != null
                ? Ok(new { message = "RoadMapPoint added", roadMap = result })
                : StatusCode(500, new { message = "RoadMapPoint adding error" });
        }

        [HttpPut]
        [Route("Edit/{roadMapPointId:int}")]
        public async Task<IActionResult> EditAsync(int roadMapPointId, [FromBody] RoadMapPointDto roadMapPoint)
        {
            var result = await _roadMapPointService.EditAsync(roadMapPointId, roadMapPoint);
            return result != null
                ? Ok(new { message = "RoadMapPoint edited", roadMap = result })
                : StatusCode(500, new { message = "RoadMapPoint editing error" });
        }

        [HttpDelete]
        [Route("Delete/{roadMapPointId:int}")]
        public async Task<IActionResult> Delete(int roadMapPointId)
        {
            return await _roadMapPointService.DeleteAsync(roadMapPointId) 
                ? Ok(new { message = "RoadMapPoint deleted", roadMap = roadMapPointId })
                : NotFound(new { message = "RoadMapPoint not found", roadMap = roadMapPointId });
        }


        [HttpGet]
        [Route("GetByRoadMapId/{roadMapId:int}")]
        public async Task<IActionResult> GetByRoadMapId(int roadMapId)
        {
            var roadMapPoints = await _roadMapPointService.GetByRoadMapIdAsync(roadMapId);
            if (!roadMapPoints.Any())
                return Ok(new List<RoadMapPointDto>());
            return Ok(roadMapPoints);
        }
    }
}
