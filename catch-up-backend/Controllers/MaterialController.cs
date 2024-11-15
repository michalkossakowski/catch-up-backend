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
            var material = await _materialService.CreateMaterialAsync(materialDto);
            return CreatedAtAction(nameof(Get), new { materialId = material.Id }, new { message = "Material  created", material });
        }

        /// <summary>
        /// Only use to change material Name. 
        /// To add/remove files use AddFile/RemoveFile
        /// </summary>
        /// <param name="materialId">To find material</param>
        /// <param name="name"> name - value that is changed</param>
        /// <returns>On success, returns the message</returns>
        [HttpPut]
        [Route("Edit/{materialId:int}/{name}")]
        public async Task<IActionResult> Edit(int materialId, string name )
        {
            await _materialService.EditAsync(materialId, name);
            return Ok(new { message = "Material  edited"});
        }

        /// <summary>
        /// It will change state of material to Deleted and 
        /// all of relations (in table fileInMaterial) to the state deleted
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns>On success, returns the message</returns>
        [HttpDelete]
        [Route("Delete/{materialId:int}")]
        public async Task<IActionResult> Delete(int materialId)
        {
            await _materialService.DeleteAsync(materialId);
            return Ok(new { message = "Material  deleted" });
        }

        /// <summary>
        /// It will change state of material to Archived and 
        /// all of relations (in table fileInMaterial) to the state deleted
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns>On success, returns the message</returns>
        [HttpDelete]
        [Route("Archive/{materialId:int}")]
        public async Task<IActionResult> Archive(int materialId)
        {
            await _materialService.ArchiveAsync(materialId);
            return Ok(new { message = "Material  deleted" });
        }

        /// <summary>
        /// It add relation between material and file in table "FileInMaterial"
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="fileId"></param>
        /// <returns>On success, returns the message</returns>
        [HttpPost]
        [Route("AddFile/{materialId:int}/{fileId:int}")]
        public async Task<IActionResult> AddFile(int materialId, int fileId)
        {
            await _materialService.AddFileAsync(materialId, fileId);
            return Ok(new { message = "Added File to material",});
        }

        /// <summary>
        /// It "remove file" in material by changing state of their relation to Archived 
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RemoveFile/{materialId:int}/{fileId:int}")]
        public async Task<IActionResult> RemoveFile(int materialId, int fileId)
        {
            await _materialService.RemoveFileAsync(materialId, fileId);
            return Ok(new { message = "File Removed" });
        }

        /// <summary>
        /// Give material Id to get material (only With Active state).
        /// To get not empty List<FileDto> use GetMaterialWithFiles()
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns>
        /// MaterialDto {Id, Name, (is empty)List<FileDto>}
        /// /// </returns>
        [HttpGet]
        [Route("Get/{materialId:int}")]
        public async Task<IActionResult> Get(int materialId)
        {
            var materialDto = await _materialService.GetMaterialAsync(materialId);
            return Ok(new { message = "Material found", materialDto });
        }

        /// <summary>
        /// Use to get material with list of files which have their Id, Name, Type, Source 
        /// (only where relation state is Active)
        /// To download one of this files use Download from FileController.
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns>
        /// MaterialDto {Id, Name, List<FileDto>{Id, Name, Type: Empty, Source: Empty}}
        /// </returns>
        [HttpGet]
        [Route("GetWithFiles/{materialId:int}")]
        public async Task<IActionResult> GetWithFiles(int materialId)
        {
            var materialDto = await _materialService.GetFilesInMaterialAsync(materialId);
            return Ok(new { message = "Material found", materialDto });
        }

        /// <summary>
        /// Use to get all materials (only Active materials)
        /// </summary>
        /// <returns>
        /// List of materials whit empty List<FileDto> inside
        /// </returns>
        [HttpGet]
        [Route("GetAllMaterials")]
        public async Task<List<MaterialDto>> GetAllMaterials()
        {
            return await _materialService.GetMaterialsAync();
        }
    }
}
