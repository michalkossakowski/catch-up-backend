using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// It save file into the storage
        /// and make relation with material
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="materialId"></param>
        /// <returns> fileDto and the same materialID</returns>
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, int? materialId)
        {
            var fileDto = await _fileService.UploadFileAsync(file, materialId);
            return Ok(new { message = "File uploaded ", fileDto, materialId });
        }

        /// <summary>
        /// It physically delete file in storage, it not remove file record in database.
        /// File record change state to Deleted
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete/{fileId:int}")]
        public async Task<IActionResult> Delete(int fileId)
        {
            await _fileService.DeleteFileAsync(fileId);
            return Ok(new { message = "File deleted"});
        }

        /// <summary>
        /// It change state of file in database to Archived
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("Archive/{fileId:int}")]
        public async Task<IActionResult> Archive(int fileId)
        {
            await _fileService.ArchiveFileAsync(fileId);
            return Ok(new { message = "File archived" });
        }

        /// <summary>
        /// Use to get fileDto by fileId (only in Active state)
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get/{fileId:int}")]
        public async Task<IActionResult> Get(int fileId)
        {
            var fileDto = await _fileService.GetByIdAsync(fileId);
            return Ok(new { message = "File found", fileDto });
        }

        /// <summary>
        /// Download file by returning stream
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>Stream with the file saved in storage</returns>
        [HttpGet]
        [Route("Download/{fileId:int}")]
        public async Task<IActionResult> Download(int fileId)
        {
            var fileDto = await _fileService.GetByIdAsync(fileId);
            var stream = await _fileService.DownloadFileAsync(fileId);
            return File(stream, fileDto.Type, fileDto.Name);
        }
    }
}
