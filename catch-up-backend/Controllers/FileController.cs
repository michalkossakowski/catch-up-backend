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
        /// Uploads a file to storage and optionally associates it with a material.
        /// The file is saved in storage with a unique name, and its details are stored in the database.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <param name="materialId">Optional. The ID of the material to associate with the uploaded file.</param>
        /// <returns>A response containing the file details (FileDto) and the associated material ID, if any.</returns>
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, int? materialId)
        {
            var fileDto = await _fileService.UploadFile(file, materialId);
            if (fileDto == null)
                return NotFound(new { message = $"File not found" });
            return Ok(new { fileDto, materialId });
        }

        /// <summary>
        /// Marks a file as deleted in the database and removes it from storage.
        /// This does not delete the database record, only changes its state to "Deleted".
        /// </summary>
        /// <param name="fileId">The ID of the file to delete.</param>
        /// <returns>A success message if the file was deleted, or a not-found message if the file does not exist.</returns>
        [HttpDelete]
        [Route("DeleteAsync/{fileId:int}")]
        public async Task<IActionResult> Delete(int fileId)
        {
            return await _fileService.DeleteFile(fileId)
                ? Ok(new { message = "File deleted successfully." })
                : NotFound(new { message = "File not found." });
        }

        /// <summary>
        /// Archives a file by changing its state in the database to "Archived".
        /// The file remains in storage and can be restored if necessary.
        /// </summary>
        /// <param name="fileId">The ID of the file to archive.</param>
        /// <returns>A success message if the file was archived, or a not-found message if the file does not exist.</returns>
        [HttpPatch]
        [Route("Archive/{fileId:int}")]
        public async Task<IActionResult> Archive(int fileId)
        {
            return await _fileService.ArchiveFile(fileId)
                ? Ok(new { message = "File archived successfully." })
                : NotFound(new { message = "File not found." });
        }

        /// <summary>
        /// Retrieves the details of a file by its ID, only if the file is in an "Active" state.
        /// </summary>
        /// <param name="fileId">The ID of the file to retrieve.</param>
        /// <returns>The details of the file (FileDto) if found and active, or a not-found message otherwise.</returns>
        [HttpGet]
        [Route("Get/{fileId:int}")]
        public async Task<IActionResult> Get(int fileId)
        {
            var fileDto = await _fileService.GetById(fileId);
            if (fileDto == null)
                NotFound(new { message = "File not found." });
            return Ok(fileDto);
        }

        /// <summary>
        /// Retrieves all files that are in an "Active" state.
        /// </summary>
        /// <returns>A list of FileDto objects representing all active files.</returns>
        [HttpGet]
        [HttpGet]
        [Route("GetAllFiles")]
        public async Task<IActionResult> GetAllFiles()
        {
            var filesDto = await _fileService.GetAllFiles();
            return Ok(filesDto);
        }

        /// <summary>
        /// Downloads a file by its ID, returning the file's stream from storage.
        /// The file must be in an "Active" state to be downloaded.
        /// </summary>
        /// <param name="fileId">The ID of the file to download.</param>
        /// <returns>
        /// A file stream containing the file's content if found and active, 
        /// or a not-found message if the file does not exist or is not active.
        /// </returns>
        [HttpGet]
        [Route("Download/{fileId:int}")]
        public async Task<IActionResult> Download(int fileId)
        {
            var fileDto = await _fileService.GetById(fileId);
            if (fileDto == null)
                NotFound(new { message = "File not found." });
            var stream = await _fileService.DownloadFile(fileId);
            if (stream == null)
                NotFound(new { message = "File not found." });
            return File(stream, fileDto.Type, fileDto.Name);
        }
    }
}
