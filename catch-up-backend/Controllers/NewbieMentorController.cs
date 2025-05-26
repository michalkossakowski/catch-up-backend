using catch_up_backend.Models;
using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using catch_up_backend.Enums;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewbieMentorController : ControllerBase
    {
        private readonly INewbieMentorService _newbieMentorService;

        public NewbieMentorController(INewbieMentorService newbieMentorService)
        {
            _newbieMentorService = newbieMentorService;
        }

        // Assign a new employee to a mentor
        [HttpPost]
        [Route("Assign/{newbieId:guid}/{mentorId:guid}")]
        public async Task<IActionResult> Assign(Guid newbieId, Guid mentorId)
        {
            bool result = await _newbieMentorService.AssignNewbieToMentor(newbieId, mentorId);
            if (!result)
            {
                return NotFound(new { message = "Mentor or new employee not found or their type is invalid" });
            }
            return Ok(new { message = "New employee assigned to mentor", newbieId, mentorId });
        }

        // Set assignment state (archive or delete)
        [HttpPatch]
        [Route("SetState/{newbieId:guid}/{mentorId:guid}")]
        public async Task<IActionResult> SetState(Guid newbieId, Guid mentorId, [FromQuery] StateEnum state)
        {
            if (state != StateEnum.Archived && state != StateEnum.Deleted)
            {
                return BadRequest(new { message = "State must be Archived or Deleted" });
            }

            bool result = await _newbieMentorService.SetAssignmentState(newbieId, mentorId, state);
            if (!result)
            {
                return NotFound(new { message = "Assignment not found" });
            }

            return Ok(new { message = $"Assignment state set to {state}", newbieId, mentorId });
        }

        // Get assignments for a mentor or a new employee
        [HttpGet]
        [Route("GetAssignments/{id:guid}")]
        public async Task<IActionResult> GetAssignments(Guid id, [FromQuery] RoleEnum role)
        {
            var result = await _newbieMentorService.GetAssignments(id, role);
            if (result == null)
            {
                return NotFound(new { message = $"No assignments found for {role} with ID {id}" });
            }

            return Ok(result);
        }

        // Get users (mentors or new employees, assigned or unassigned)
        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers([FromQuery] RoleEnum role, [FromQuery] bool? assigned, [FromQuery] Guid? relatedId)
        {
            var result = await _newbieMentorService.GetUsers(role, assigned, relatedId);
            if (result == null)
            {
                return NotFound(new { message = $"No {role.ToString().ToLower()}s found" });
            }

            return Ok(result);
        }

        // Get assignment history (archived or deleted)
        [HttpGet]
        [Route("GetAssignmentHistory")]
        public async Task<IActionResult> GetAssignmentHistory([FromQuery] StateEnum? state)
        {
            var history = await _newbieMentorService.GetAssignmentHistory(state);
            if (history == null)
            {
                return NotFound(new { message = "No assignment history found" });
            }

            return Ok(history);
        }
    }
}
