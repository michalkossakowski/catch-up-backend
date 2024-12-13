using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }
        /// <summary>
        /// Creates a new material.
        /// </summary>
        /// <param name="materialDto"> Contains the data needed to create the material.The material name is required.</param>
        /// <returns>
        /// A message with the material details if creation is successful,
        /// or an error message if the creation fails. 
        /// </returns>
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(MaterialDto materialDto)
        {
            var material = await _materialService.CreateMaterial(materialDto);
            if(material == null)
                return StatusCode(500, new { message = "Material adding error" });
            return CreatedAtAction(nameof(Get), new { materialId = material.Id }, material);
        }
        /// <summary>
        /// Updates the name of an existing material.
        /// </summary>
        /// <param name="materialId">The ID of the material to update.</param>
        /// <param name="name">The new name for the material.</param>
        /// <returns> A success message if the material is updated, or an error message if the material is not found. </returns>
        [HttpPut]
        [Route("Edit/{materialId:int}/{name}")]
        public async Task<IActionResult> Edit(int materialId, string name )
        {
            return  await _materialService.Edit(materialId, name)
                ? Ok(new { message = "Material edited successfully"})
                : NotFound(new { message = "Material not found." });
        }
        /// <summary>
        /// Deletes a material by marking it as deleted.
        /// </summary>
        /// <param name="materialId">The ID of the material to delete.</param>
        /// <returns>A success message if the material is deleted, or an error message if the material is not found.</returns>
        [HttpDelete]
        [Route("Delete/{materialId:int}")]
        public async Task<IActionResult> Delete(int materialId)
        {
            return await _materialService.Delete(materialId)
                ? Ok(new { message = "Material deleted successfully" })
                : NotFound(new { message = "Material not found." });
        }

        /// <summary>
        /// Archives a material by marking it as archived.
        /// </summary>
        /// <param name="materialId">The ID of the material to archive.</param>
        /// <returns>A success message if the material is archived, or an error message if the material is not found.</returns>>
        [HttpDelete]
        [Route("Archive/{materialId:int}")]
        public async Task<IActionResult> Archive(int materialId)
        {
            return await _materialService.Archive(materialId)
                ? Ok(new { message = "Material archived successfully." })
                : NotFound(new { message = "Material not found." });
        }

        /// <summary>
        /// Adds a file to a material.
        /// </summary>
        /// <param name="materialId">The ID of the material to add the file to.</param>
        /// <param name="fileId">The ID of the file to add.</param>
        /// <returns>A success message if the file is added, or an error message if the file or material is not found.</returns>
        [HttpPost]
        [Route("AddFile/{materialId:int}/{fileId:int}")]
        public async Task<IActionResult> AddFile(int materialId, int fileId)
        {
            return await _materialService.AddFile(materialId, fileId)
                ? Ok(new { message = "File added to material." })
                : NotFound(new { message = "File or Material not found." });

        }

        /// <summary>
        /// Removes a file from a material.
        /// </summary>
        /// <param name="materialId">The ID of the material to remove the file from.</param>
        /// <param name="fileId">The ID of the file to remove.</param>
        /// <returns>A success message if the file is removed, or an error message if the file or material is not found.</returns>
        [HttpPost]
        [Route("RemoveFile/{materialId:int}/{fileId:int}")]
        public async Task<IActionResult> RemoveFile(int materialId, int fileId)
        {
            return await _materialService.RemoveFile(materialId, fileId)
                ? Ok(new { message = "File removed from material successfully." })
                : NotFound(new { message = "File or material not found." });
        }

        /// <summary>
        /// Retrieves a material by its ID.
        /// </summary>
        /// <param name="materialId">The ID of the material to retrieve.</param>
        /// <returns>The material details if found, or an error message if not found.</returns>
        [HttpGet]
        [Route("Get/{materialId:int}")]
        public async Task<IActionResult> Get(int materialId)
        {
            var materialDto = await _materialService.GetMaterial(materialId);
            if (materialDto == null)
            {
                return NotFound(new { message = "Material not found." });
            }
            return Ok(materialDto);
        }

        /// <summary>
        /// Retrieves a material along with its associated files.
        /// </summary>
        /// <param name="materialId">The ID of the material to retrieve along with its files.</param>
        /// <returns>The material details with associated files, or an error message if not found.</returns>
        [HttpGet]
        [Route("GetWithFiles/{materialId:int}")]
        public async Task<IActionResult> GetWithFiles(int materialId)
        {
            var materialDto = await _materialService.GetFilesInMaterial(materialId);
            if (materialDto == null)
            {
                return NotFound(new { message = "Material not found." });
            }
            return Ok(materialDto);
        }

        /// <summary>
        /// Retrieves all active materials.
        /// </summary>
        /// <returns>A list of all active materials.</returns>
        [HttpGet]
        [Route("GetAllMaterials")]
        public async Task<IActionResult> GetAllMaterials()
        {
            var materials = await _materialService.GetMaterials();
            return Ok(materials);
        }
    }
}
