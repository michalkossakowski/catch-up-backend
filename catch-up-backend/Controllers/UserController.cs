using catch_up_backend.Dtos;
using catch_up_backend.Database;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly CatchUpDbContext _context;
        public UserController(CatchUpDbContext context) 
        { 
            _context = context;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] UserDto newUser)
        {
            var user = new UserModel(
                newUser.Name,
                newUser.Surname,
                newUser.Email,
                newUser.Password,
                newUser.Type,
                newUser.Position
                );
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "User added", user = newUser });
        }
    }
}
