using catch_up_backend.Dtos;
using catch_up_backend.Helpers;
using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BadgeController : ControllerBase
    {
        private readonly IBadgeService _badgeService;
        public BadgeController(IBadgeService badgeService)
        {
            _badgeService = badgeService;
        }

        [HttpPost]
        [Route("Add")]
        [Authorize(Policy = "HROrAdmin")]
        public async Task<IActionResult> Add([FromBody] BadgeDto newBadge)
        {
            return await _badgeService.Add(newBadge)
                ? Ok(new { message = "Badge added", badge = newBadge })
                : StatusCode(500, new { message = "Error: Badge add"});
        }

        [HttpPut]
        [Route("Edit/{badgeId:int}")]
        [Authorize(Policy = "HROrAdmin")]
        public async Task<IActionResult> Edit(int badgeId, [FromBody] BadgeDto newBadge)
        {
            return await _badgeService.Edit(badgeId, newBadge)
                ? Ok(new { message = "Badge edited", badge = newBadge })
                : StatusCode(500, new { message = "Error: Badge edit" });
        }

        [HttpDelete]
        [Route("Delete/{badgeId:int}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int badgeId)
        {
            return await _badgeService.Delete(badgeId)
                ? Ok(new { message = "Badge deleted", badge = badgeId })
                : NotFound(new { message = "Error: Badge delete", badge =  badgeId});
        }

        [HttpGet]
        [Route("GetById/{badgeId:int}")]
        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> GetById(int badgeId)
        {
            var badge = await _badgeService.GetById(badgeId);
            if (badge == null)
                return NotFound(new { message = $"Badge with id: {badgeId} not found" });
            return Ok(badge);
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> GetAll()
        {
            var badge = await _badgeService.GetAll();
            return Ok(badge);
        }

        [HttpPost]
        [Route("AssignManualBadge")]
        [Authorize(Policy = "HROrAdmin")]
        public async Task<IActionResult> AssignManualBadge([FromQuery] Guid mentorId, [FromQuery] int badgeId)
        {
            try
            {
                await _badgeService.AssignBadgeManuallyAsync(mentorId, badgeId);
                return Ok(new { message = $"Badge {badgeId} has been manually assigned to mentor {mentorId}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error assigning badge manually", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetByMentorId")]
        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> GetByMentorId()
        {
            var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);
            var mentorBadges = await _badgeService.GetByMentorId(userId);

            return Ok(mentorBadges);
        }
    }
}
