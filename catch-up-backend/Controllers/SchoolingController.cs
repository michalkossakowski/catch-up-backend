using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolingController : ControllerBase
    {
        private readonly ISchoolingService _schoolingService;
        public SchoolingController(ISchoolingService schoolingService)
        {
            _schoolingService = schoolingService;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] SchoolingDto schoolingDto)
        {
            var fullSchoolingDto = await _schoolingService.CreateSchooling(schoolingDto);

            if (fullSchoolingDto.Schooling.Id == 0)
            {
                throw new Exception("Id not set for the created Schooling");
            }
            return CreatedAtAction("GetFull", new { schoolingId = fullSchoolingDto.Schooling.Id }, new { message = "Schooling  created",data = fullSchoolingDto });
        }


        [HttpGet]
        [Route("GetFull/{schoolingId:int}")]
        public async Task<IActionResult> GetFull(int schoolingId)
        {
            var fullSchoolingDto = await _schoolingService.GetFull(schoolingId);
            return Ok(new { message = "Schooling found", fullSchoolingDto });
        }

        [HttpGet]
        [Route("GetAllFull")]
        public async Task<IActionResult> GetAllFull()
        {
            var schoolings = await _schoolingService.GetAllFull();
            return Ok(new { message = "Schoolings retrieved successfully", data = schoolings });
        }

        [HttpGet]
        [Route("GetUserSchoolingsID/{userId:Guid}")]
        public async Task<IActionResult> GetUserSchoolingsID(Guid userId)
        {
            var schoolingsIds = await _schoolingService.GetUserSchoolingsID(userId);
            return Ok(new { message = "Schoolings retrieved successfully", data = schoolingsIds });
        }

        [HttpGet]
        [Route("GetAllFull/{userId:Guid}")]
        public async Task<IActionResult> GetAllFull(Guid userId)
        {
            var schoolings = await _schoolingService.GetAllFull(userId);
            return Ok(new { message = "Schoolings retrieved successfully", data = schoolings });
        }

        [HttpDelete]
        [Route("DeleteSchooling/{schoolingId:int}")]
        public async Task<IActionResult> DeleteSchooling(int schoolingId)
        {
            await _schoolingService.DeleteSchooling(schoolingId);
            return Ok(new { message = "Schooling deleted successfully" });
        }

        [HttpDelete]
        [Route("ArchiveSchooling/{schoolingId:int}")]
        public async Task<IActionResult> ArchiveSchooling(int schoolingId)
        {
            await _schoolingService.ArchiveSchooling(schoolingId);
            return Ok(new { message = "Schooling archived successfully" });
        }

        // Do dokończenia
        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] FullSchoolingDto fullSchoolingDto)
        {
            await _schoolingService.Edit(fullSchoolingDto);
            return Ok(new { message = "Schooling updated successfully" });
        }

        [HttpPost]
        [Route("AddSchoolingPart/{schoolingId:int}")]
        public async Task<IActionResult> AddSchoolingPart(int schoolingId, [FromBody] SchoolingPartDto schoolingPartDto)
        {
            await _schoolingService.AddSchoolingPart(schoolingPartDto, schoolingId);
            return Ok(new { message = "Schooling part added successfully" });
        }

        [HttpPost]
        [Route("AddSchoolingToUser/{userId:Guid}/{schoolingId:int}")]
        public async Task<IActionResult> AddSchoolingToUser(Guid userId, int schoolingId)
        {
            await _schoolingService.AddSchoolingToUser(userId, schoolingId);
            return Ok(new { message = "Added schooling to user" });
        }

        [HttpPost]
        [Route("ArchiveUserSchooling/{userId:Guid}/{schoolingId:int}")]
        public async Task<IActionResult> ArchiveUserSchooling(Guid userId, int schoolingId)
        {
            await _schoolingService.ArchiveUserSchooling(userId, schoolingId);
            return Ok(new { message = "Schooling successfully archived" });
        }

        [HttpPost]
        [Route("DeleteUserSchooling/{userId:Guid}/{schoolingId:int}")]
        public async Task<IActionResult> DeleteUserSchooling(Guid userId, int schoolingId)
        {
            await _schoolingService.DeleteUserSchooling(userId, schoolingId);
            return Ok(new { message = "Schooling successfully deleted" });
        }
    }
}
