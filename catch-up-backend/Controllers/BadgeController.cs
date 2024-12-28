using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BadgeController : ControllerBase
    {
        private readonly IBadgeService _badgeService;
        public BadgeController(IBadgeService badgeService)
        {
            _badgeService = badgeService;
        }

        [HttpPost]
        [Route("AddAsync")]
        public async Task<IActionResult> Add([FromBody] BadgeDto newBadge)
        {
            return await _badgeService.Add(newBadge)
                ? Ok(new { message = "Badge added", badge = newBadge })
                : StatusCode(500, new { message = "Error: Badge add"});
        }

        [HttpPut]
        [Route("EditAsync/{badgeId:int}")]
        public async Task<IActionResult> Edit(int badgeId, [FromBody] BadgeDto newBadge)
        {
            return await _badgeService.Edit(badgeId, newBadge)
                ? Ok(new { message = "Badge edited", badge = newBadge })
                : StatusCode(500, new { message = "Error: Badge edit" });
        }

        [HttpDelete]
        [Route("DeleteAsync/{badgeId:int}")]
        public async Task<IActionResult> Delete(int badgeId)
        {
            return await _badgeService.Delete(badgeId)
                ? Ok(new { message = "Badge deleted", badge = badgeId })
                : NotFound(new { message = "Error: Badge delete", badge =  badgeId});
        }

        [HttpGet]
        [Route("GetById/{badgeId:int}")]
        public async Task<IActionResult> GetById(int badgeId)
        {
            var badge = await _badgeService.GetById(badgeId);
            if (badge == null)
                return NotFound(new { message = $"Badge with id: {badgeId} not found" });
            return Ok(badge);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var badge = await _badgeService.GetAll();
            if (!badge.Any())
                return NotFound(new { message = "No Badges found" });
            return Ok(badge);
        }

        [HttpPost]
        [Route("AssignManualBadge")]
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
        [Route("GetByMentorId/{userId:guid}")]
        public async Task<IActionResult> GetByMentorId(Guid userId)
        {
            var mentorBadges = await _badgeService.GetByMentorId(userId);
            if (mentorBadges == null || !mentorBadges.Any())
                return NotFound(new { message = $"Mentor with id: {userId} has no assigned badges" });
            return Ok(mentorBadges);
        }
    }
}
