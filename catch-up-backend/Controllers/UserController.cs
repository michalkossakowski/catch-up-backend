using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] UserDto newUser)
        {
            var addedUser = await _userService.Add(newUser);
            return Ok(new { message = "User added", user = addedUser });
        }

        [HttpPatch]
        [Route("Edit/{userId}")]
        public async Task<IActionResult> Edit(Guid userId, [FromBody] UserDto updatedUser)
        {
            await _userService.Edit(userId, updatedUser);
            return Ok(new { message = "User updated", user = updatedUser });
        }

        [HttpDelete]
        [Route("Delete/{userId}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            await _userService.Delete(userId);
            return Ok(new { message = "User deleted" });
        }

        [HttpGet]
        [Route("GetById/{userId}")]
        public async Task<IActionResult> GetById(Guid userId)
        {
            var user = await _userService.GetById(userId);
            if (user == null)
                return NotFound(new { message = $"User with id: [{userId}] not found" });
            return Ok(user);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetRole/{userId}")]
        public async Task<IActionResult> GetRole(Guid userId)
        {
            var role = await _userService.GetRole(userId);
            return Ok(role);
        }

        [HttpGet]
        [Route("GetMentorAdmin")]
        public async Task<IActionResult> GetMentorAdmin()
        {
            var users = await _userService.GetMentorAdmin();
            return Ok(users);
        }
    }
}