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


        /// <summary>
        /// Retrieves a complete schooling object (`FullSchooling`) with all its parts by its ID.
        /// A `FullSchooling` includes detailed information about the schooling and its related parts,
        /// such as topics, materials, and associated metadata (Files).
        /// </summary>
        /// <param name="schoolingId">The ID of the schooling to retrieve.</param>
        /// <returns>Full schooling details if found, or a NotFound result.</returns>
        [HttpGet]
        [Route("GetFull/{schoolingId:int}")]
        public async Task<IActionResult> GetFull(int schoolingId)
        {
            var fullSchooling = await _schoolingService.GetFull(schoolingId);
            return fullSchooling != null
                ? Ok(fullSchooling)
                : NotFound(new { message = "Schooling not found." });
        }
        /// <summary>
        /// Retrieves all schooling objects (`FullSchooling`) with their complete details.
        /// Each `FullSchooling` includes information such as the schooling's structure,
        /// associated parts, and detailed materials.
        /// </summary>
        /// <returns>List of all full schooling details.</returns>
        [HttpGet]
        [Route("GetAllFull")]
        public async Task<IActionResult> GetAllFull()
        {
            var fullSchoolings = await _schoolingService.GetAllFull();
            return Ok(fullSchoolings);
        }
        /// <summary>
        /// Retrieves the IDs of all schoolings associated with a specific user.
        /// This can be used to determine which schoolings a user is currently enrolled in.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>List of schooling IDs associated with the user.</returns>
        [HttpGet]
        [Route("GetUserSchoolingsID/{userId:Guid}")]
        public async Task<IActionResult> GetUserSchoolingsID(Guid userId)
        {
            var schoolingsId = await _schoolingService.GetUserSchoolingsID(userId);
            return Ok(schoolingsId);
        }
        /// <summary>
        /// Retrieves all schooling objects (`FullSchooling`) associated with a specific user,
        /// including their structure, parts, and other detailed information.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>List of full schooling details for the user.</returns>
        [HttpGet]
        [Route("GetAllFull/{userId:Guid}")]
        public async Task<IActionResult> GetAllFull(Guid userId)
        {
            var fullSchoolings = await _schoolingService.GetAllFull(userId);
            return Ok(fullSchoolings);
        }
        /// <summary>
        /// Retrieves all schooling parts available in the system.
        /// Schooling parts represent individual components or sections of a schooling,
        /// such as content, files
        /// </summary>
        /// <returns>List of all schooling parts.</returns>
        [HttpGet]
        [Route("GetAllSchoolingParts")]
        public async Task<IActionResult> GetAllSchoolingParts()
        {
            var schoolingsParts = await _schoolingPartService.GetAllSchoolingParts();
            return Ok(schoolingsParts);
        }
        /// <summary>
        /// Retrieves schooling part of passed Id.
        /// Schooling parts represent individual components or sections of a schooling,
        /// such as content, files
        /// </summary>
        /// <returns>One schooling part.</returns>
        [HttpGet]
        [Route("GetSchoolingPart/{schoolingPartId:int}")]
        public async Task<IActionResult> GetSchoolingPart(int schoolingPartId)
        {
            var schoolingsPart = await _schoolingPartService.GetSchoolingPart(schoolingPartId);
            if(schoolingsPart == null)
                return NotFound(new { message = "Schooling part not found." });
            return Ok(schoolingsPart);

        }
        /// <summary>
        /// Updates an entire schooling, including its metadata and all associated parts.
        /// (exclude files), if you want update category just change category id in "schooling", not in "category")
        /// Synchronizes the relationships between the schooling parts and their materials.
        /// </summary>
        /// <param name="fullSchoolingDto">The data transfer object containing the updated schooling and its parts.</param>
        /// <returns>Success or failure of the operation.</returns>
        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] FullSchoolingDto fullSchoolingDto)
        {
            return await _schoolingService.Edit(fullSchoolingDto)
                ? Ok(new { message = "Schooling updated successfully." })
                : NotFound(new { message = "Schooling not found." });
        }

        /// <summary>
        /// Updates the metadata of an existing schooling (title, description, priority, and category).
        /// Ensures the associated category is active before applying changes.
        /// </summary>
        /// <param name="schoolingDto">The data transfer object containing the updated metadata.</param>
        /// <returns>Success or failure of the operation.</returns>
        [HttpPut]
        [Route("EditSchooling")]
        public async Task<IActionResult> EditSchooling([FromBody] SchoolingDto schoolingDto)
        {
            return await _schoolingService.EditSchooling(schoolingDto)
                ? Ok(new { message = "Schooling updated successfully." })
                : NotFound(new { message = "Schooling not found." });
        }

        /// <summary>
        /// Updates the details of a specific schooling part, such as its name, content, and materials (exclude files).
        /// Synchronizes the list of associated materials, adding new ones and removing outdated ones.
        /// </summary>
        /// <param name="schoolingPartDto">The data transfer object containing the updated schooling part.</param>
        /// <returns>Success or failure of the operation.</returns>
        [HttpPut]
        [Route("EditSchoolingPart")]
        public async Task<IActionResult> EditSchoolingPart([FromBody] SchoolingPartDto schoolingPartDto)
        {
            return await _schoolingPartService.EditSchoolingPart(schoolingPartDto)
                ? Ok(new { message = "Schooling updated successfully." })
                : NotFound(new { message = "Schooling not found." });
        }
        /// <summary>
        /// Creates a new schooling by calling the service method to add the schooling to the database.
        /// If the provided category ID is invalid or the category doesn't exist, the operation will fail.
        /// </summary>
        /// <param name="schoolingDto">The data transfer object containing the details of the schooling to be created.</param>
        /// <returns>
        /// If successful, returns a Created response with the created schooling data. Otherwise, returns a BadRequest response.
        /// </returns>
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] SchoolingDto schoolingDto)
        {
            var fullSchooling = await _schoolingService.CreateSchooling(schoolingDto);

            return fullSchooling != null && fullSchooling.Schooling.Id != 0
                ? CreatedAtAction("GetFull", new { schoolingId = fullSchooling.Schooling.Id }, fullSchooling)
                : BadRequest(new { message = "Failed to create schooling." });
        }
        /// <summary>
        /// Adds a new part to an existing schooling, such as name and content, and associates any materials with it.
        /// </summary>
        /// <param name="schoolingId">The ID of the schooling to which the part will be added.</param>
        /// <param name="schoolingPartDto">The data transfer object containing the details of the schooling part to be added.</param>
        /// <returns>If successful, returns a Created response with created schooling part. Otherwise, returns a NotFound response.</returns>
        [HttpPost]
        [Route("AddSchoolingPart/{schoolingId:int}")]
        public async Task<IActionResult> CreateSchoolingPart(int schoolingId, [FromBody] SchoolingPartDto schoolingPartDto)
        {
            var schoolingPart = await _schoolingService.CreateSchoolingPart(schoolingPartDto, schoolingId);

            return schoolingPart != null && schoolingPart.Id != 0
                ? CreatedAtAction("GetSchoolingPart", new { schoolingPartId = schoolingPart.Id }, schoolingPart)
                : BadRequest(new { message = "Failed to create schooling part." });
        }
        /// <summary>
        /// Associates a schooling with a user. Adds the schooling to the user's list of schoolings.
        /// </summary>
        /// <param name="userId">The ID of the user to associate the schooling with.</param>
        /// <param name="schoolingId">The ID of the schooling to be added to the user.</param>
        /// <returns>If successful, returns an Ok response. Otherwise, returns a NotFound response.</returns>
        [HttpPost]
        [Route("AddSchoolingToUser/{userId:Guid}/{schoolingId:int}")]
        public async Task<IActionResult> AddSchoolingToUser(Guid userId, int schoolingId)
        {
            return await _schoolingService.AddSchoolingToUser(userId, schoolingId)
                ? Ok(new { message = "Added schooling to user successfully." })
                : NotFound(new { message = "User or schooling not found." });
        }
        /// <summary>
        /// Adds a material to an existing schooling part. If the material or part is invalid, the operation will fail.
        /// </summary>
        /// <param name="shoolingPartId">The ID of the schooling part to which the material will be added.</param>
        /// <param name="materialId">The ID of the material to be associated with the schooling part.</param>
        /// <returns>If successful, returns an Ok response. Otherwise, returns a NotFound response.</returns>
        [HttpPost]
        [Route("AddMaterialToSchooling/{shoolingPartId:int}/{materialId:int}")]
        public async Task<IActionResult> AddMaterialToSchooling(int shoolingPartId, int materialId)
        {
            return await _schoolingPartService.AddMaterialToSchooling(shoolingPartId, materialId)
                ? Ok(new { message = "Added material to schooling part successfully." })
                : NotFound(new { message = "Schooling part or material not found." });
        }
        /// <summary>
        /// Archives a user's association with a specific schooling. The user's schooling will be marked as archived in the database.
        /// </summary>
        /// <param name="userId">The ID of the user whose schooling association is being archived.</param>
        /// <param name="schoolingId">The ID of the schooling to be archived for the user.</param>
        /// <returns>If successful, returns an Ok response. Otherwise, returns a NotFound response.</returns>
        [HttpDelete]
        [Route("ArchiveUserSchooling/{userId:Guid}/{schoolingId:int}")]
        public async Task<IActionResult> ArchiveUserSchooling(Guid userId, int schoolingId)
        {
           return await _schoolingService.ArchiveUserSchooling(userId, schoolingId)
                ? Ok(new { message = "User schooling archived successfully." })
                : NotFound(new { message = "User or schooling not found." });
        }
        /// <summary>
        /// Deletes a user's association with a specific schooling. The user's schooling will be marked as deleted in the database.
        /// </summary>
        /// <param name="userId">The ID of the user whose schooling association is being deleted.</param>
        /// <param name="schoolingId">The ID of the schooling to be deleted for the user.</param>
        /// <returns>If successful, returns an Ok response. Otherwise, returns a NotFound response.</returns>
        [HttpDelete]
        [Route("DeleteUserSchooling/{userId:Guid}/{schoolingId:int}")]
        public async Task<IActionResult> DeleteUserSchooling(Guid userId, int schoolingId)
        {
            return await _schoolingService.DeleteUserSchooling(userId, schoolingId)
                ? Ok(new { message = "User schooling deleted successfully." })
                : NotFound(new { message = "User or schooling not found." });
        }
        /// <summary>
        /// Archives a schooling part. The part will be marked as archived in the database, along with its associated materials (intermediate table).
        /// </summary>
        /// <param name="schoolingPartId">The ID of the schooling part to be archived.</param>
        /// <returns>If successful, returns an Ok response. Otherwise, returns a NotFound response.</returns>
        [HttpDelete]
        [Route("ArchiveSchoolingPart/{schoolingPartId:int}")]
        public async Task<IActionResult> ArchiveSchoolingPart(int schoolingPartId)
        {
            return await _schoolingPartService.ArchiveSchoolingPart(schoolingPartId)
                ? Ok(new { message = "Schooling part archived successfully." })
                : NotFound(new { message = "Schooling part not found." });
        }
        /// <summary>
        /// Deletes a schooling part. The part will be marked as deleted in the database, and its associated
        /// (intermediate table) materials will also be deleted.
        /// </summary>
        /// <param name="schoolingPartId">The ID of the schooling part to be deleted.</param>
        /// <returns>If successful, returns an Ok response. Otherwise, returns a NotFound response.</returns>
        [HttpDelete]
        [Route("DeleteSchoolingPart/{schoolingPartId:int}")]
        public async Task<IActionResult> DeleteSchoolingPart(int schoolingPartId)
        {
            return await _schoolingPartService.DeleteSchoolingPart(schoolingPartId)
                ? Ok(new { message = "Schooling part deleted successfully." })
                : NotFound(new { message = "Schooling part not found." });
        }
        /// <summary>
        /// Deletes a specific schooling. The schooling and all its associated parts and materials (intermediate table schoolingPartMaterial) 
        /// will be marked as deleted in the database.
        /// </summary>
        /// <param name="schoolingId">The ID of the schooling to be deleted.</param>
        /// <returns>If successful, returns an Ok response. Otherwise, returns a NotFound response.</returns>
        [HttpDelete]
        [Route("DeleteSchooling/{schoolingId:int}")]
        public async Task<IActionResult> DeleteSchooling(int schoolingId)
        {
            return await _schoolingService.DeleteSchooling(schoolingId)
                ? Ok(new { message = "Schooling deleted successfully." })
                : NotFound(new { message = "Schooling not found." });
        }
        /// <summary>
        /// Archives a specific schooling. The schooling and all its associated parts and materials (intermediate table schoolingPartMaterial) 
        /// will be marked as archived in the database.
        /// </summary>
        /// <param name="schoolingId">The ID of the schooling to be archived.</param>
        /// <returns>If successful, returns an Ok response. Otherwise, returns a NotFound response.</returns>
        [HttpDelete]
        [Route("ArchiveSchooling/{schoolingId:int}")]
        public async Task<IActionResult> ArchiveSchooling(int schoolingId)
        {
            return await _schoolingService.ArchiveSchooling(schoolingId)
                ? Ok(new { message = "Schooling archived successfully." })
                : NotFound(new { message = "Schooling not found." });
        }
        /// <summary>
        /// Archives a specific schooling. The schooling and all its associated parts and materials 
        /// (intermediate table schoolingPartMaterial) will be marked as archived in the database.
        /// </summary>
        /// <param name="schoolingId">The ID of the schooling to be archived.</param>
        /// <returns>If successful, returns an Ok response. Otherwise, returns a NotFound response.</returns>
        [HttpDelete]
        [HttpDelete]
        [Route("ArchiveMaterialFromSchooling/{schoolingPartId:int}/{materialId:int}")]
        public async Task<IActionResult> ArchiveMaterialFromSchooling(int schoolingPartId, int materialId)
        {
            return await _schoolingPartService.ArchiveMaterialFromSchooling(schoolingPartId, materialId)
                ? Ok(new { message = "Material archived successfully from the schooling part." })
                : NotFound(new { message = "Material or schooling part not found." });

        }
        /// <summary>
        /// Deletes a specific material from a schooling part. 
        /// The material (intermediate table schoolingPartMaterial) will be marked as deleted in the database.
        /// </summary>
        /// <param name="schoolingPartId">The ID of the schooling part containing the material to be deleted.</param>
        /// <param name="materialId">The ID of the material to be deleted.</param>
        /// <returns>If successful, returns an Ok response. Otherwise, returns a NotFound response.</returns>
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