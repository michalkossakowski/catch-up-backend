using catch_up_backend.Dtos;
using catch_up_backend.Helpers;
using catch_up_backend.Interfaces;
using catch_up_backend.Response;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> Get(int schoolingId)
        {
            var schooling = await _schoolingService.GetById(schoolingId);
            return schooling != null
                ? Ok(schooling)
                : NotFound(new { message = "Schooling not found." });
        }

        [HttpGet]
        [Route("GetUserSchooling/{schoolingId:int}")]
        public async Task<IActionResult> GetUserSchooling(int schoolingId)
        {
            var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);
            var schooling = await _schoolingService.GetById(schoolingId, userId);

            return schooling != null
                ? Ok(schooling)
                : NotFound(new { message = "Schooling not found." });
        }

        [HttpGet]
        [Route("GetSchoolingPart/{schoolingPartId:int}")]
        public async Task<IActionResult> GetSchoolingPart(int schoolingPartId)
        {
            var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);
            var schoolingsPart = await _schoolingPartService.GetSchoolingPart(schoolingPartId, userId);
            if (schoolingsPart == null)
                return NotFound(new { message = "Schooling part not found." });
            return Ok(schoolingsPart);
        }

        [HttpPatch]
        [Route("ChangeUserSchoolingPartState/{schoolingUserId:int}/{schoolingPartId:int}/{state:bool}")]
        public async Task<IActionResult> ChangeUserSchoolingPartState(int schoolingUserId, int schoolingPartId, bool state)
        {
            return await _schoolingPartService.ChangeUserSchoolingPartState(schoolingUserId, schoolingPartId, state)
                ? Ok(new { message = "Schooling part state changed successfully." })
                : NotFound(new { message = "Schooling part not found." });
        }
        [HttpPut]
        [Route("EditSchoolingPart")]
        public async Task<IActionResult> EditSchoolingPart([FromBody] SchoolingPartUpdateDto schoolingPartDto)
        {
            return await _schoolingPartService.EditSchoolingPart(schoolingPartDto)
                ? Ok(new { message = "Schooling updated successfully." })
                : NotFound(new { message = "Schooling not found." });
        }

        [HttpPut]
        [Route("EditSchooling")]
        public async Task<IActionResult> EditSchooling([FromBody] SchoolingDto schoolingDto)
        {
            return await _schoolingService.EditSchooling(schoolingDto)
                ? Ok(new { message = "Schooling updated successfully." })
                : NotFound(new { message = "Schooling not found." });
        }
        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<PagedResponse<SchoolingDto>>> GetSchoolings([FromQuery] SchoolingQueryParameters parameters, [FromQuery] string mode = "all")
        {
            if (parameters.PageNumber < 1 || parameters.PageSize < 1)
            {
                return BadRequest("PageNumber and PageSize must be greater than 0.");
            }

            var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);

            var result = mode switch
            {
                "owned" => await _schoolingService.GetOwnedSchoolingsAsync(parameters, userId),
                "subscribed" => await _schoolingService.GetSubscribedSchoolingsAsync(parameters, userId),
                _ => await _schoolingService.GetSchoolingsAsync(parameters),
            };
            return Ok(result);
        }
    }
}