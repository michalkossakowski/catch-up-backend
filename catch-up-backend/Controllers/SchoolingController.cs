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
        private readonly ISchoolingPartService _schoolingPartService;

        public SchoolingController(ISchoolingService schoolingService, ISchoolingPartService schoolingPartService)
        {
            _schoolingService = schoolingService;
            _schoolingPartService = schoolingPartService;
        }

        [HttpGet]
        [Route("GetFull/{schoolingId:int}")]
        public async Task<IActionResult> GetFull(int schoolingId)
        {
            var fullSchoolingDto = await _schoolingService.GetFull(schoolingId);
            return Ok(new { message = "Schooling found", fullSchoolingDto });
        }

        [HttpGet]
        [Route("GetAllFull")]
        public async Task<IActionResult> GetAllFull()
        {
            var schoolings = await _schoolingService.GetAllFull();
            return Ok(new { message = "Schoolings retrieved successfully", data = schoolings });
        }

        [HttpGet]
        [Route("GetUserSchoolingsID/{userId:Guid}")]
        public async Task<IActionResult> GetUserSchoolingsID(Guid userId)
        {
            var schoolingsIds = await _schoolingService.GetUserSchoolingsID(userId);
            return Ok(new { message = "Creator of schooling id retrieved successfully", data = schoolingsIds });
        }

        [HttpGet]
        [Route("GetAllFull/{userId:Guid}")]
        public async Task<IActionResult> GetAllFull(Guid userId)
        {
            var schoolings = await _schoolingService.GetAllFull(userId);
            return Ok(new { message = "Schoolings retrieved successfully", data = schoolings });
        }

        [HttpGet]
        [Route("GetAllSchoolingParts")]
        public async Task<IActionResult> GetAllSchoolingParts()
        {
            var schoolingsParts = await _schoolingPartService.GetAllSchoolingParts();
            return Ok(new { message = "Schoolings parts retrieved successfully", data = schoolingsParts });
        }

        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] FullSchoolingDto fullSchoolingDto)
        {
            await _schoolingService.Edit(fullSchoolingDto);
            return Ok(new { message = "Schooling updated successfully" });
        }

        [HttpPut]
        [Route("EditSchooling")]
        public async Task<IActionResult> EditSchooling([FromBody] SchoolingDto schoolingDto)
        {
            await _schoolingService.EditSchooling(schoolingDto);
            return Ok(new { message = "Schooling updated successfully" });
        }

        [HttpPut]
        [Route("EditSchoolingPart")]
        public async Task<IActionResult> EditSchoolingPart([FromBody] SchoolingPartDto schoolingPartDto)
        {
            await _schoolingPartService.EditSchoolingPart(schoolingPartDto);
            return Ok(new { message = "Schooling updated successfully" });
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] SchoolingDto schoolingDto)
        {
            var fullSchoolingDto = await _schoolingService.CreateSchooling(schoolingDto);

            if (fullSchoolingDto.Schooling.Id == 0)
            {
                throw new Exception("Id not set for the created Schooling");
            }
            return CreatedAtAction("GetFull", new { schoolingId = fullSchoolingDto.Schooling.Id }, new { message = "Schooling  created", data = fullSchoolingDto });
        }

        [HttpPost]
        [Route("AddSchoolingPart/{schoolingId:int}")]
        public async Task<IActionResult> AddSchoolingPart(int schoolingId, [FromBody] SchoolingPartDto schoolingPartDto)
        {
            await _schoolingService.AddSchoolingPart(schoolingPartDto, schoolingId);
            return Ok(new { message = "Schooling part added successfully" });
        }

        [HttpPost]
        [Route("AddSchoolingToUser/{userId:Guid}/{schoolingId:int}")]
        public async Task<IActionResult> AddSchoolingToUser(Guid userId, int schoolingId)
        {
            await _schoolingService.AddSchoolingToUser(userId, schoolingId);
            return Ok(new { message = "Added schooling to user" });
        }

        [HttpPost]
        [Route("AddMaterialToSchooling/{shoolingPartId:int}/{materialId:int}")]
        public async Task<IActionResult> AddMaterialToSchooling(int shoolingPartId, int materialId)
        {
            await _schoolingPartService.AddMaterialToSchooling(shoolingPartId, materialId);
            return Ok(new { message = "Added material to schooling part" });
        }

        [HttpDelete]
        [Route("ArchiveUserSchooling/{userId:Guid}/{schoolingId:int}")]
        public async Task<IActionResult> ArchiveUserSchooling(Guid userId, int schoolingId)
        {
            await _schoolingService.ArchiveUserSchooling(userId, schoolingId);
            return Ok(new { message = "User archived in schoolig" });
        }

        [HttpDelete]
        [Route("DeleteUserSchooling/{userId:Guid}/{schoolingId:int}")]
        public async Task<IActionResult> DeleteUserSchooling(Guid userId, int schoolingId)
        {
            await _schoolingService.DeleteUserSchooling(userId, schoolingId);
            return Ok(new { message = "User deleted in schoolig" });
        }

        [HttpDelete]
        [Route("ArchiveSchoolingPart/{schoolingPartId:int}")]
        public async Task<IActionResult> ArchiveSchoolingPart(int schoolingPartId)
        {
            await _schoolingPartService.ArchiveSchoolingPart(schoolingPartId);
            return Ok(new { message = "Schooling part archived" });
        }

        [HttpDelete]
        [Route("DeleteSchoolingPart/{schoolingPartId:int}")]
        public async Task<IActionResult> DeleteSchoolingPart(int schoolingPartId)
        {
            await _schoolingPartService.DeleteSchoolingPart(schoolingPartId);
            return Ok(new { message = "Schooling part deleted" });
        }

        [HttpDelete]
        [Route("DeleteSchooling/{schoolingId:int}")]
        public async Task<IActionResult> DeleteSchooling(int schoolingId)
        {
            await _schoolingService.DeleteSchooling(schoolingId);
            return Ok(new { message = "Schooling deleted successfully" });
        }

        [HttpDelete]
        [Route("ArchiveSchooling/{schoolingId:int}")]
        public async Task<IActionResult> ArchiveSchooling(int schoolingId)
        {
            await _schoolingService.ArchiveSchooling(schoolingId);
            return Ok(new { message = "Schooling archived successfully" });
        }

        [HttpDelete]
        [Route("ArchiveMaterialFromSchooling/{schoolingPartId:int}/{materialId:int}")]
        public async Task<IActionResult> ArchiveMaterialFromSchooling(int schoolingPartId, int materialId)
        {
            await _schoolingPartService.ArchiveMaterialFromSchooling(schoolingPartId, materialId);
            return Ok(new { message = "Material from schooling part has been archived" });
        }

        [HttpDelete]
        [Route("DeleteMaterialFromSchooling/{schoolingPartId:int}/{materialId:int}")]
        public async Task<IActionResult> DeleteMaterialFromSchooling(int schoolingPartId, int materialId)
        {
            await _schoolingPartService.DeleteMaterialFromSchooling(schoolingPartId, materialId);
            return Ok(new { message = "Material from schooling part has been deleted" });
        }
    }
}