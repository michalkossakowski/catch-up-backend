using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface IFileService
    {
        public Task<FileDto> UploadFile(IFormFile newFile, int? materialID);
        public Task DeleteFile(int fileId);
        public Task<FileDto> GetById(int fileId);
        public Task<Stream> DownloadFile(int fileId);
        public Task AddToMaterial(int fileId, int materialId);
        public Task<List<FileDto>> GetFiles(int materialId);
        public Task ArchiveFile(int fileId);
        public Task<List<FileDto>> GetAllFiles();
    }
}
