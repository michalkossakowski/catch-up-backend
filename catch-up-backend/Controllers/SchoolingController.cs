using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Helpers;
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
        // Done
        [HttpGet]
        [Route("Get/{schoolingId:int}")]
        public async Task<IActionResult> Get(int schoolingId)
            {
            var schooling = await _schoolingService.GetById(schoolingId);
            return schooling != null
                ? Ok(schooling)
                : NotFound(new { message = "Schooling not found." });
        }

        [HttpGet]
        [Route("GetUserSchooling/{schoolingId:int}")]
        public async Task<IActionResult> GetUserSchooling(int schoolingId)
        {
            var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);
            var schooling = await _schoolingService.GetById(schoolingId, userId);

            return schooling != null
                ? Ok(schooling)
                : NotFound(new { message = "Schooling not found." });
        }

        [HttpGet]
        [Route("GetSchoolingPart/{schoolingPartId:int}")]
        public async Task<IActionResult> GetSchoolingPart(int schoolingPartId)
        {
            var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);
            var schoolingsPart = await _schoolingPartService.GetSchoolingPart(schoolingPartId, userId);
            if (schoolingsPart == null)
                return NotFound(new { message = "Schooling part not found." });
            return Ok(schoolingsPart);
        }

        [HttpPatch]
        [Route("ChangeUserSchoolingPartState/{schoolingUserId:int}/{schoolingPartId:int}/{state:bool}")]
        public async Task<IActionResult> ChangeUserSchoolingPartState(int schoolingUserId, int schoolingPartId, bool state)
        {
            return await _schoolingPartService.ChangeUserSchoolingPartState(schoolingUserId,schoolingPartId, state)
                ? Ok(new { message = "Schooling part state changed successfully." })
                : NotFound(new { message = "Schooling part not found." });
        }
        [HttpPut]
        [Route("EditSchoolingPart")]
        public async Task<IActionResult> EditSchoolingPart([FromBody] SchoolingPartUpdateDto schoolingPartDto)
        {
            return await _schoolingPartService.EditSchoolingPart(schoolingPartDto)
                ? Ok(new { message = "Schooling updated successfully." })
                : NotFound(new { message = "Schooling not found." });
        }

        //Change
        [HttpGet]
        [Route("GetAllFull")]
        public async Task<IActionResult> GetAllFull()
        {
            var fullSchoolings = await _schoolingService.GetAllFull();
            return Ok(fullSchoolings);
        }
       
        [HttpGet]
        [Route("GetUserSchoolingsID/{userId:Guid}")]
        public async Task<IActionResult> GetUserSchoolingsID(Guid userId)
        {
            var schoolingsId = await _schoolingService.GetUserSchoolingsID(userId);
            return Ok(schoolingsId);
        }

        [HttpGet]
        [Route("GetAllFull/{userId:Guid}")]
        public async Task<IActionResult> GetAllFull(Guid userId)
        {
            var fullSchoolings = await _schoolingService.GetAllFull(userId);
            return Ok(fullSchoolings);
        }

        [HttpGet]
        [Route("GetAllSchoolingParts")]
        public async Task<IActionResult> GetAllSchoolingParts()
        {
            var schoolingsParts = await _schoolingPartService.GetAllSchoolingParts();
            return Ok(schoolingsParts);
        }
         
        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] FullSchoolingDto fullSchoolingDto)
        {
            return await _schoolingService.Edit(fullSchoolingDto)
                ? Ok(new { message = "Schooling updated successfully." })
                : NotFound(new { message = "Schooling not found." });
        }

        [HttpPut]
        [Route("EditSchooling")]
        public async Task<IActionResult> EditSchooling([FromBody] SchoolingDto schoolingDto)
        {
            return await _schoolingService.EditSchooling(schoolingDto)
                ? Ok(new { message = "Schooling updated successfully." })
                : NotFound(new { message = "Schooling not found." });
        }       
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] SchoolingDto schoolingDto)
        {
            var fullSchooling = await _schoolingService.CreateSchooling(schoolingDto);

            return fullSchooling != null && fullSchooling.Schooling.Id != 0
                ? CreatedAtAction("GetFull", new { schoolingId = fullSchooling.Schooling.Id }, fullSchooling)
                : BadRequest(new { message = "Failed to create schooling." });
        }
      
        [HttpPost]
        [Route("AddSchoolingPart/{schoolingId:int}")]
        public async Task<IActionResult> CreateSchoolingPart(int schoolingId, [FromBody] SchoolingPartDto schoolingPartDto)
        {
            var schoolingPart = await _schoolingService.CreateSchoolingPart(schoolingPartDto, schoolingId);

            return schoolingPart != null && schoolingPart.Id != 0
                ? CreatedAtAction("GetSchoolingPart", new { schoolingPartId = schoolingPart.Id }, schoolingPart)
                : BadRequest(new { message = "Failed to create schooling part." });
        }
       
        [HttpPost]
        [Route("AddSchoolingToUser/{userId:Guid}/{schoolingId:int}")]
        public async Task<IActionResult> AddSchoolingToUser(Guid userId, int schoolingId)
        {
            return await _schoolingService.AddSchoolingToUser(userId, schoolingId)
                ? Ok(new { message = "Added schooling to user successfully." })
                : NotFound(new { message = "User or schooling not found." });
        }
        
        [HttpPost]
        [Route("AddMaterialToSchooling/{shoolingPartId:int}/{materialId:int}")]
        public async Task<IActionResult> AddMaterialToSchooling(int shoolingPartId, int materialId)
        {
            return await _schoolingPartService.AddMaterialToSchooling(shoolingPartId, materialId)
                ? Ok(new { message = "Added material to schooling part successfully." })
                : NotFound(new { message = "Schooling part or material not found." });
        }
      
        [HttpDelete]
        [Route("ArchiveUserSchooling/{userId:Guid}/{schoolingId:int}")]
        public async Task<IActionResult> ArchiveUserSchooling(Guid userId, int schoolingId)
        {
           return await _schoolingService.ArchiveUserSchooling(userId, schoolingId)
                ? Ok(new { message = "User schooling archived successfully." })
                : NotFound(new { message = "User or schooling not found." });
        }
       
        [HttpDelete]
        [Route("DeleteUserSchooling/{userId:Guid}/{schoolingId:int}")]
        public async Task<IActionResult> DeleteUserSchooling(Guid userId, int schoolingId)
        {
            return await _schoolingService.DeleteUserSchooling(userId, schoolingId)
                ? Ok(new { message = "User schooling deleted successfully." })
                : NotFound(new { message = "User or schooling not found." });
        }
        
        [HttpDelete]
        [Route("ArchiveSchoolingPart/{schoolingPartId:int}")]
        public async Task<IActionResult> ArchiveSchoolingPart(int schoolingPartId)
        {
            return await _schoolingPartService.ArchiveSchoolingPart(schoolingPartId)
                ? Ok(new { message = "Schooling part archived successfully." })
                : NotFound(new { message = "Schooling part not found." });
        }
       
        [HttpDelete]
        [Route("DeleteSchoolingPart/{schoolingPartId:int}")]
        public async Task<IActionResult> DeleteSchoolingPart(int schoolingPartId)
        {
            return await _schoolingPartService.DeleteSchoolingPart(schoolingPartId)
                ? Ok(new { message = "Schooling part deleted successfully." })
                : NotFound(new { message = "Schooling part not found." });
        }
        
        [HttpDelete]
        [Route("DeleteSchooling/{schoolingId:int}")]
        public async Task<IActionResult> DeleteSchooling(int schoolingId)
        {
            return await _schoolingService.DeleteSchooling(schoolingId)
                ? Ok(new { message = "Schooling deleted successfully." })
                : NotFound(new { message = "Schooling not found." });
        }
        
        [HttpDelete]
        [Route("ArchiveSchooling/{schoolingId:int}")]
        public async Task<IActionResult> ArchiveSchooling(int schoolingId)
        {
            return await _schoolingService.ArchiveSchooling(schoolingId)
                ? Ok(new { message = "Schooling archived successfully." })
                : NotFound(new { message = "Schooling not found." });
        }
       
        [HttpDelete]
        [Route("ArchiveMaterialFromSchooling/{schoolingPartId:int}/{materialId:int}")]
        public async Task<IActionResult> ArchiveMaterialFromSchooling(int schoolingPartId, int materialId)
        {
            return await _schoolingPartService.ArchiveMaterialFromSchooling(schoolingPartId, materialId)
                ? Ok(new { message = "Material archived successfully from the schooling part." })
                : NotFound(new { message = "Material or schooling part not found." });

        }
      
        [HttpDelete]
        [Route("DeleteMaterialFromSchooling/{schoolingPartId:int}/{materialId:int}")]
        public async Task<IActionResult> DeleteMaterialFromSchooling(int schoolingPartId, int materialId)
        {
            return await _schoolingPartService.DeleteMaterialFromSchooling(schoolingPartId, materialId)
                ? Ok(new { message = "Material deleted successfully from the schooling part." })
                : NotFound(new { message = "Material or schooling part not found." });
        }
    }
}