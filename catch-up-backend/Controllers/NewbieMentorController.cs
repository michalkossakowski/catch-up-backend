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

        // Przypisanie nowego pracownika do mentora
        [HttpPost("Assign/{newbieId:guid}/{mentorId:guid}")]
        public async Task<IActionResult> Assign(Guid newbieId, Guid mentorId)
        {
            bool result = await _newbieMentorService.AssignNewbieToMentor(newbieId, mentorId);
            if (!result)
                return NotFound(new { message = "Mentor or User not found or their Type is wrong" });
            return Ok(new { message = "Newbie assigned to mentor", newbieId, mentorId });
        }

        // Usuwanie przypisania nowego pracownika do mentora
        [HttpDelete("Delete/{newbieId:guid}/{mentorId:guid}")]
        public async Task<IActionResult> Delete(Guid newbieId, Guid mentorId)
        {
            bool result = await _newbieMentorService.Delete(newbieId, mentorId);
            if (!result)
                return NotFound(new { message = "Assignment not found" });
            return Ok(new { message = "Newbie unassigned from mentor", newbieId, mentorId });
        }

        // Jeden endpoint do pobierania przypisań (mentor/newbie)
        [HttpGet("GetAssignments")]
        public async Task<IActionResult> GetAssignments([FromQuery] string role, [FromQuery] Guid id)
        {
            IEnumerable<UserModel> assignments;
            if (role.ToLower() == "mentor")
                assignments = await _newbieMentorService.GetAssignmentsByMentor(id);
            else if (role.ToLower() == "newbie")
                assignments = await _newbieMentorService.GetAssignmentsByNewbie(id);
            else
                return BadRequest(new { message = "Invalid role" });

            var result = new
            {
                assignments,
                count = assignments?.Count() ?? 0
            };
            return Ok(result);
        }

        // Jeden endpoint do pobierania wszystkich użytkowników z parametrami rola i assigned
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] TypeEnum role, [FromQuery] bool? assigned = null, [FromQuery] Guid? relatedId = null)
        {
            IEnumerable<UserModel> users = null;

            switch (role)
            {
            case TypeEnum.Mentor:

                    if (assigned == true && relatedId.HasValue)
                    users = await _newbieMentorService.GetAssignmentsByNewbie(relatedId.Value);
                else if (assigned == false && relatedId.HasValue)
                    users = await _newbieMentorService.GetAllUnassignedMentors(relatedId.Value);
                else
                    users = await _newbieMentorService.GetAllMentors();
                    break;

            case TypeEnum.Newbie:
            
                if (assigned == true && relatedId.HasValue)
                    users = await _newbieMentorService.GetAssignmentsByMentor(relatedId.Value);
                else if (assigned == false && relatedId.HasValue)
                    users = await _newbieMentorService.GetAllUnassignedNewbies(relatedId.Value);
                else if (assigned == false)
                    users = await _newbieMentorService.GetAllUnassignedNewbies();
                else
                    users = await _newbieMentorService.GetAllNewbies();
                break;
            default:
                    return BadRequest(new { message = "Invalid role" });
            }


            var result = new
            {
                users,
                count = users?.Count() ?? 0
            };
            return Ok(result);
        }
    }
}