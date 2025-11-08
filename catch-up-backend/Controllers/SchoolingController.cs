using catch_up_backend.Dtos;
using catch_up_backend.Helpers;
using catch_up_backend.Interfaces;
using catch_up_backend.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SchoolingController : ControllerBase
    {
        private readonly ISchoolingService _schoolingService;
        private readonly ISchoolingPartService _schoolingPartService;
        public SchoolingController(ISchoolingService schoolingService, ISchoolingPartService schoolingPartService)
        {
            _schoolingService = schoolingService;
            _schoolingPartService = schoolingPartService;
        }
        // Done
        [HttpGet]
        [Route("Get/{schoolingId:int}")]
        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> Get(int schoolingId)
        {
            var schooling = await _schoolingService.GetById(schoolingId);
            return schooling != null
                ? Ok(schooling)
                : NotFound(new { message = "Schooling not found." });
        }

        

        [HttpGet]
        [Route("GetSchoolingPart/{schoolingPartId:int}")]
        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> GetSchoolingPart(int schoolingPartId)
        {
            var schoolingsPart = await _schoolingPartService.GetSchoolingPart(schoolingPartId);
            if (schoolingsPart == null)
                return NotFound(new { message = "Schooling part not found." });
            return Ok(schoolingsPart);
        }

        [HttpPut]
        [Route("EditSchoolingPart")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> EditSchoolingPart([FromBody] SchoolingPartDto schoolingPartDto)
        {
            return await _schoolingPartService.EditSchoolingPart(schoolingPartDto)
                ? Ok(new { message = "Schooling updated successfully." })
                : NotFound(new { message = "Schooling not found." });
        }

        [HttpPut]
        [Route("EditSchooling")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> EditSchooling([FromBody] SchoolingDto schoolingDto)
        {
            return await _schoolingService.EditSchoolingAsync(schoolingDto)
                ? Ok(new { message = "Schooling updated successfully." })
                : NotFound(new { message = "Schooling not found." });
        }
        [HttpGet]
        [Route("Get")]
        [Authorize(Policy = "AnyRole")]
        public async Task<ActionResult<PagedResponse<SchoolingDto>>> GetSchoolings([FromQuery] SchoolingQueryParameters parameters, [FromQuery] string mode = "all")
        {
            if (parameters.PageNumber < 1 || parameters.PageSize < 1)
            {
                return BadRequest("PageNumber and PageSize must be greater than 0.");
            }

            var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);

            var result = await _schoolingService.GetSchoolingsAsync(parameters);

            return Ok(result);
        }
        [HttpDelete]
        [Route("Delete/{schoolingId:int}")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> DeleteSchooling(int schoolingId)
        {
            return await _schoolingService.DeleteSchoolingAsync(schoolingId)
                ? Ok(new { message = "Schooling deleted successfully." })
                : NotFound(new { message = "Schooling not found." });
        }
        [HttpPost]
        [Route("CreateSchooling")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> CreateSchooling([FromBody] SchoolingDto schoolingDto)
        {
            var createdSchooling = await _schoolingService.CreateSchoolingAsync(schoolingDto);
            return createdSchooling != null
                ? Ok(createdSchooling)
                : BadRequest(new { message = "Failed to create schooling." });
        }
    }
}