using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface IFileService
    {
        public Task<FileDto> UploadFileAsync(IFormFile newFile, int? materialID);
        public Task DeleteFileAsync(int fileId);
        public Task<FileDto> GetByIdAsync(int fileId);
        public Task<Stream> DownloadFileAsync(int fileId);
        public Task AddToMaterialAsync(int fileId, int materialId);
        public Task<List<FileDto>> GetFilesAsync(int materialId);
        public Task ArchiveFileAsync(int fileId);
        public Task<List<FileDto>> GetAllFiles();
    }
}
