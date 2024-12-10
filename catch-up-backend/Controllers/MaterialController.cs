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
        /// It create new material
        /// </summary>
        /// <param name="materialDto">It takes materialDto, 
        /// material name is required to create new material
        /// </param>
        /// <returns>message and materialDto</returns>
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(MaterialDto materialDto)
        {
            var material = await _materialService.CreateMaterial(materialDto);
            return CreatedAtAction(nameof(Get), new { materialId = material.Id }, material);
        }

        /// <summary>
        /// Updates the name of an existing material.
        /// </summary>
        /// <param name="materialId">The ID of the material to update.</param>
        /// <param name="name">The new name for the material.</param>
        /// <returns>Success message.</returns>
        [HttpPut]
        [Route("Edit/{materialId:int}/{name}")]
        public async Task<IActionResult> Edit(int materialId, string name )
        {
            await _materialService.Edit(materialId, name);
            return Ok(new { message = "Material  edited"});
        }

        /// <summary>
        /// Deletes a material by marking it as deleted.
        /// </summary>
        /// <param name="materialId">The ID of the material to delete.</param>
        /// <returns>Success message.</returns>
        [HttpDelete]
        [Route("Delete/{materialId:int}")]
        public async Task<IActionResult> Delete(int materialId)
        {
            await _materialService.Delete(materialId);
            return Ok(new { message = "Material  deleted" });
        }

        /// <summary>
        /// Archives a material by marking it as archived.
        /// </summary>
        /// <param name="materialId">The ID of the material to archive.</param>
        /// <returns>Success message.</returns>
        [HttpDelete]
        [Route("Archive/{materialId:int}")]
        public async Task<IActionResult> Archive(int materialId)
        {
            await _materialService.Archive(materialId);
            return Ok(new { message = "Material  deleted" });
        }

        /// <summary>
        /// Adds a file to a material.
        /// </summary>
        /// <param name="materialId">The ID of the material.</param>
        /// <param name="fileId">The ID of the file to add.</param>
        /// <returns>Success message.</returns>
        [HttpPost]
        [Route("AddFile/{materialId:int}/{fileId:int}")]
        public async Task<IActionResult> AddFile(int materialId, int fileId)
        {
            await _materialService.AddFile(materialId, fileId);
            return Ok(new { message = "Added File to material",});
        }

        /// <summary>
        /// Removes a file from a material.
        /// </summary>
        /// <param name="materialId">The ID of the material.</param>
        /// <param name="fileId">The ID of the file to remove.</param>
        /// <returns>Success message.</returns>
        [HttpPost]
        [Route("RemoveFile/{materialId:int}/{fileId:int}")]
        public async Task<IActionResult> RemoveFile(int materialId, int fileId)
        {
            await _materialService.RemoveFile(materialId, fileId);
            return Ok(new { message = "File removed from material successfully." });
        }

        /// <summary>
        /// Retrieves a material by ID.
        /// </summary>
        /// <param name="materialId">The ID of the material.</param>
        /// <returns>The material details.</returns>
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
        /// Retrieves a material with its associated files.
        /// </summary>
        /// <param name="materialId">The ID of the material.</param>
        /// <returns>The material details with associated files.</returns>
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
