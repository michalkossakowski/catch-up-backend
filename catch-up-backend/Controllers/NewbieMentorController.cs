using catch_up_backend.Models;
using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        [Route("Assign")]
        public async Task<IActionResult> Assign([FromBody] NewbieMentorModel newAssignment)
        {
            await _newbieMentorService.AssignNewbieToMentor(newAssignment);
            return Ok(new { message = "Newbie assigned to mentor", assignment = newAssignment });
        }

        // Edycja przypisania pracownika do mentora (bardzo uniwersalny. Dosłownie zastępuje stare połączenie nowym)
        [HttpPut]
        [Route("Edit/{newbieId:guid}/{mentorId:guid}")]
        public async Task<IActionResult> Edit(Guid newbieId, Guid mentorId, [FromBody] NewbieMentorModel updatedAssignment)
        {
            bool result = await _newbieMentorService.EditAssignment(newbieId, mentorId, updatedAssignment);
            if (!result)
                return NotFound(new { message = "Assignment not found" });

            return Ok(new { message = "Assignment updated", assignment = updatedAssignment });
        }

        [HttpPut]
        [Route("EditStatus/{newbieId:guid}/{mentorId:guid}/{status:bool}")]
        public async Task<IActionResult> EditStatus(Guid newbieId, Guid mentorId,  bool status )
        {
            bool result = await _newbieMentorService.EditStatus(newbieId, mentorId, status);
            if (!result)
            {
                return NotFound(new { message = "Assignment not found" });
            }

            return Ok(new { message = "Status updated", assignment = status });
        }
        // Usuwanie przypisania nowego pracownika do mentora
        [HttpDelete]
        [Route("Unassign/{newbieId:guid}/{mentorId:guid}")]
        public async Task<IActionResult> Unassign(Guid newbieId, Guid mentorId)
        {
            bool result = await _newbieMentorService.UnassignNewbieFromMentor(newbieId, mentorId);
            if (!result)
                return NotFound(new { message = "Assignment not found" });

            return Ok(new { message = "Newbie unassigned from mentor", newbieId, mentorId });
        }

        // Pobieranie wszystkich przypisań dla danego mentora
        [HttpGet]
        [Route("GetAssignmentsByMentor/{mentorId:guid}")]
        public async Task<IActionResult> GetAssignmentsByMentor(Guid mentorId)
        {
            IEnumerable<NewbieMentorModel> assignments = await _newbieMentorService.GetAssignmentsByMentor(mentorId);
            if (!assignments.Any())
                return NotFound(new { message = $"No assignments found for mentor with ID {mentorId}" });

            return Ok(assignments);
        }

        // Pobieranie przypisań dla danego nowego pracownika
        [HttpGet]
        [Route("GetAssignmentsByNewbie/{newbieId:guid}")]
        public async Task<IActionResult> GetAssignmentsByNewbie(Guid newbieId)
        {
            NewbieMentorModel assignment = await _newbieMentorService.GetAssignmentByNewbie(newbieId);
            if (assignment == null)
                return NotFound(new { message = $"No assignments found for newbie with ID {newbieId}" });

            return Ok(assignment);
        }
        [HttpGet("GetStatus/{newbieId}/{mentorId}")]
        public async Task<ActionResult<bool>> GetBool(Guid newbieId, Guid mentorId)
        {
            var isActive = await _newbieMentorService.GetStatus(newbieId, mentorId);

            if (isActive == null)
            {
                return NotFound("No connection found for the provided Newbie and Mentor IDs.");
            }

            return Ok(isActive);
        }
    }
}