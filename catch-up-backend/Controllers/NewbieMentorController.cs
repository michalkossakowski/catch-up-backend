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
        [HttpPost]
        [Route("Assign/{newbieId:guid}/{mentorId:guid}")]
        public async Task<IActionResult> Assign(Guid newbieId, Guid mentorId)
        {
            bool result = await _newbieMentorService.AssignNewbieToMentor(newbieId, mentorId);
            if (!result)
            {
                return NotFound(new { message = "Mentor or User not found or their Type is wrong" });
            }
            return Ok(new { message = "Newbie assigned to mentor", newbieId, mentorId });
        }

        // Archiwizuje przypisania nowego pracownika do mentora
        [HttpPut]
        [Route("Archive/{newbieId:guid}/{mentorId:guid}")]
        public async Task<IActionResult> Archive(Guid newbieId, Guid mentorId)
        {
            bool result = await _newbieMentorService.Archive(newbieId, mentorId);
            if (!result)
            {
                return NotFound(new { message = "Assignment not found" });
            }

            return Ok(new { message = "Connection has been archived", newbieId, mentorId });
        }
        // Usuwanie przypisania nowego pracownika do mentora
        [HttpDelete]
        [Route("Delete/{newbieId:guid}/{mentorId:guid}")]
        public async Task<IActionResult> Delete(Guid newbieId, Guid mentorId)
        {
            bool result = await _newbieMentorService.Delete(newbieId, mentorId);
            if (!result)
            {
                return NotFound(new { message = "Assignment not found" });
            }

            return Ok(new { message = "Newbie unassigned from mentor", newbieId, mentorId });
        }
        // Pobieranie wszystkich przypisań dla danego mentora
        [HttpGet]
        [Route("GetAssignmentsByMentor/{mentorId:guid}")]
        public async Task<IActionResult> GetAssignmentsByMentor(Guid mentorId)
        {
            IEnumerable<NewbieMentorModel> assignments = await _newbieMentorService.GetAssignmentsByMentor(mentorId);
            if (!assignments.Any())
            {
                return NotFound(new { message = $"No assignments found for mentor with ID {mentorId}" });
            }

            return Ok(assignments);
        }

        // Pobieranie przypisań dla danego nowego pracownika
        [HttpGet]
        [Route("GetAssignmentsByNewbie/{newbieId:guid}")]
        public async Task<IActionResult> GetAssignmentsByNewbie(Guid newbieId)
        {
            IEnumerable<NewbieMentorModel> assignments = await _newbieMentorService.GetAssignmentsByNewbie(newbieId);
            if (!assignments.Any())
            {
                return NotFound(new { message = $"No assignments found for mentor with ID {newbieId}" });
            }

            return Ok(assignments);
        }
    }
}