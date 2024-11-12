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
        /// <param name="materialId"></param>
        /// <param name="name"></param>
        /// <returns>On success, returns the message</returns>
        [HttpPut]
        [Route("Edit/{materialId:int}/{name}")]
        public async Task<IActionResult> Edit(int materialId, string name )
        {
            await _materialService.EditAsync(materialId, name);
            return Ok(new { message = "Material  edited"});
        }

        /// <summary>
        /// It will delete material Id and remove its relation with files.
        /// Files will be not delete.
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

        [HttpPost]
        [Route("AddFile/{materialId:int}/{fileId:int}")]
        public async Task<IActionResult> AddFile(int materialId, int fileId)
        {
            await _materialService.AddFileAsync(materialId, fileId);
            return Ok(new { message = "Added File to material",});
        }

        [HttpPost]
        [Route("RemoveFile/{materialId:int}/{fileId:int}")]
        public async Task<IActionResult> RemoveFile(int materialId, int fileId)
        {
            await _materialService.RemoveFileAsync(materialId, fileId);
            return Ok(new { message = "File Removed" });
        }

        /// <summary>
        /// Give material Id to get material.
        /// To get not empty  List<FileDto> use GetMaterialWithFiles()
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns>
        /// MaterialDto {Id, Name, List<FileDto>{Id, Name, Type: Empty, Source: Empty}}
        /// /// </returns>
        [HttpGet]
        [Route("Get/{materialId:int}")]
        public async Task<IActionResult> Get(int materialId)
        {
            var materialDto = await _materialService.GetMaterialAsync(materialId);
            return Ok(new { message = "Material found", materialDto });
        }

        /// <summary>
        /// Use to get material with list of files which have their Id and Name (without source and type)
        /// To download this files use Download from FileController.
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
        /// Use to get all materials 
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
