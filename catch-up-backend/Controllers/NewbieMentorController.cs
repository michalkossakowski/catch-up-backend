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
        [HttpDelete]
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
            IEnumerable<UserModel> assignments = await _newbieMentorService.GetAssignmentsByMentor(mentorId);
            if (!assignments.Any())
            {
                return NotFound(new { message = $"No assignments found for mentor with ID {mentorId}" });
            }

            return Ok(assignments);
        }
        //  Zwraca liczbę powiązanych do mentora nowych pracowników
        [HttpGet]
        [Route("GetNewbieCountByMentor/{mentorId:guid}")]
        public async Task<IActionResult> GetNewbieCountByMentor(Guid mentorId)
        {
            int count = await _newbieMentorService.GetNewbieCountByMentor(mentorId);
            if (count==null)
            {
                return NotFound(new { message = $"No assignments found for mentor with ID {mentorId}" });
            }
            return Ok(count);
        }
        // Pobieranie przypisań dla danego nowego pracownika
        [HttpGet]
        [Route("GetAssignmentsByNewbie/{newbieId:guid}")]
        public async Task<IActionResult> GetAssignmentsByNewbie(Guid newbieId)
        {
            IEnumerable<UserModel> assignments = await _newbieMentorService.GetAssignmentsByNewbie(newbieId);
            if (!assignments.Any())
            {
                return NotFound(new { message = $"No assignments found for mentor with ID {newbieId}" });
            }

            return Ok(assignments);
        }
        //  Zwraca liczbę powiązanych do nowego pracownika mentorów
        [HttpGet]
        [Route("GetMentorCountByNewbie/{mentorId:guid}")]
        public async Task<IActionResult>GetMentorsCountByNewbie(Guid newbieId)
        {
            int count = await _newbieMentorService.GetMentorsCountByNewbie(newbieId);
            if (count == null)
            {
                return NotFound(new { message = $"No assignments found for mentor with ID {newbieId}" });
            }
            return Ok(count);
        }
        // Pobieranie wszystkich zaarchiwizowanych przypisań
        [HttpGet]
        [Route("GetAllArchived")]
        public async Task<IActionResult> GetAllArchived()
        {
            IEnumerable<NewbieMentorModel> archived = await _newbieMentorService.GetAllArchived();
            if (!archived.Any())
            {
                return NotFound(new { message = "There aren't archived connections." });
            }
            return Ok(archived);
        }
        // Pobieranie wszystkich usuniętych przypisań
        [HttpGet]
        [Route("GetAllDeleted")]
        public async Task<IActionResult> GetAllDeleted()
        {
            IEnumerable<NewbieMentorModel> deleted = await _newbieMentorService.GetAllDeleted();
            if (!deleted.Any())
            {
                return NotFound(new { message = "There aren't deleted connections." });
            }

            return Ok(deleted);
        }
        // Pobieranie wszystkich mentorów
        [HttpGet]
        [Route("GetAllMentors")]
        public async Task<IActionResult> GetAllMentors()
        {
            IEnumerable<UserModel> mentors = await _newbieMentorService.GetAllMentors();
            if (!mentors.Any())
            {
                return NotFound(new { message = $"No mentors found." });
            }

            return Ok(mentors);
        }
        // Pobieranie wszystkich nowych pracowników jeszcze nie przypisanych do konretnego mentora.
        [HttpGet]
        [Route("GetAllUnassignedNewbies/{mentorId:guid}")]
        public async Task<IActionResult> GetAllUnassignedNewbies(Guid mentorId)
        {
            IEnumerable<UserModel> unassigned = await _newbieMentorService.GetAllUnassignedNewbies(mentorId);
            if (!unassigned.Any())
            {
                return NotFound(new { message = $"No unassigned newbies found. " });
            }

            return Ok(unassigned);
        }
        // Zwraca datę rozpoczęcia szkolenia
        [HttpGet]
        [Route("GetDateStart/{newbieId:guid}/{mentorId:guid}")]
        public async Task<IActionResult> GetDateStart(Guid newbieId, Guid mentorId)
        {
            string dateStart = await _newbieMentorService.GetDateStart(newbieId, mentorId);
            if (dateStart==null)
            {
                return NotFound(new { message = $"No start datefound." });
            }

            return Ok(dateStart);
        }
        // Zwraca datę zakończenia szkolenia 
        [HttpGet]
        [Route("GetDateEnd/{newbieId:guid}/{mentorId:guid}")]
        public async Task<IActionResult> GetDateEnd(Guid newbieId, Guid mentorId)
        {
            string dateEnd = await _newbieMentorService.GetDateEnd(newbieId, mentorId);
            if (dateEnd == null)
            {
                return NotFound(new { message = $"No start datefound." });
            }

            return Ok(dateEnd);
        }
    }
}