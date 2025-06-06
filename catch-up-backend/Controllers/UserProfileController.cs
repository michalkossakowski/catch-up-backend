using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProfile(Guid userId)
        {
            var profile = await _userProfileService.GetUserProfileAsync(userId);
            
            if (profile == null)
            {
                // Return an empty profile with default values
                return Ok(new UserProfileDto
                {
                    UserId = userId,
                    Bio = string.Empty,
                    Department = string.Empty,
                    Location = string.Empty,
                    Phone = string.Empty,
                    TeamsUsername = string.Empty,
                    SlackUsername = string.Empty,
                    Interests = new List<string>(),
                    Languages = new List<string>()
                });
            }
            
            return Ok(profile);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserProfile([FromBody] UserProfileDto userProfileDto)
        {
            if (!userProfileDto.UserId.HasValue)
            {
                return BadRequest(new { field = "UserId", message = "UserId is required" });
            }

            try
            {
                var createdProfile = await _userProfileService.CreateUserProfileAsync(userProfileDto);
                return CreatedAtAction(nameof(GetUserProfile), new { userId = createdProfile.UserId }, createdProfile);
            }
            catch (ArgumentException ex)
            {
                string field = string.IsNullOrEmpty(ex.ParamName) ? "general" : ex.ParamName;
                return BadRequest(new { field, message = ex.Message });
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserProfile(Guid userId, [FromBody] UserProfileDto userProfileDto)
        {
            try
            {
                var updatedProfile = await _userProfileService.UpdateUserProfileAsync(userId, userProfileDto);
                return Ok(updatedProfile);
            }
            catch (ArgumentException ex)
            {
                string field = string.IsNullOrEmpty(ex.ParamName) ? "general" : ex.ParamName;
                return BadRequest(new { field, message = ex.Message });
            }
        }
    }
} 