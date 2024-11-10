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

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, int? materialsId)
        {
            var fileDto = await _fileService.UploadFileAsync(file, materialsId);
            return Ok(new { message = "File uploaded", fileDto });
        }

        [HttpDelete]
        [Route("Delete/{fileId:int}")]
        public async Task<IActionResult> Delete(int fileId)
        {
            await _fileService.DeleteFileAsync(fileId);
            return Ok(new { message = "File deleted"});
        }

        [HttpGet]
        [Route("Get/{fileId:int}")]
        public async Task<IActionResult> Get(int fileId)
        {
            var fileDto = await _fileService.GetByIdAsync(fileId);
            return Ok(new { message = "File found", fileDto });
        }

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
