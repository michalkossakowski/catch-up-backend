using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolingController : ControllerBase
    {
        private readonly ISchoolingService _schoolingService;
        public SchoolingController(ISchoolingService schoolingService)
        {
            _schoolingService = schoolingService;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] SchoolingDto schoolingDto)
        {
            var schooling = await _schoolingService.CreateSchoolingAsync(schoolingDto);

            if (schooling.Schooling.Id == 0)
            {
                throw new Exception("Id not set for the created Schooling");
            }
            return CreatedAtAction("GetFull", new { schoolingId = schooling.Schooling.Id }, new { message = "Schooling  created", schooling });
        }


        [HttpGet]
        [Route("GetFull/{schoolingId:int}")]
        public async Task<IActionResult> GetFullAsync(int schoolingId)
        {
            var fullSchoolingDto = await _schoolingService.GetFullAsync(schoolingId);
            return Ok(new { message = "Schooling found", fullSchoolingDto });
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var schoolings = await _schoolingService.GetAllAsync();
            return Ok(new { message = "Schoolings retrieved successfully", data = schoolings });
        }

        [HttpDelete]
        [Route("Delete/{schoolingId:int}")]
        public async Task<IActionResult> DeleteAsync(int schoolingId)
        {
            var isDeleted = await _schoolingService.DeleteAsync(schoolingId);
            return Ok(new { message = "Schooling deleted successfully" });
        }
    }
}
