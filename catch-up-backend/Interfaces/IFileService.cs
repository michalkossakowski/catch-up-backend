using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface IFileService
    {
        public Task<FileDto> UploadFile(IFormFile newFile, int? materialID, Guid? owner, DateTime? uploadDate);
        public Task<bool> DeleteFile(int fileId);
        public Task<FileDto> GetById(int fileId);
        public Task<Stream> DownloadFile(int fileId);
        public Task<bool> AddFilesToMaterial(List<int> fileIds, int materialId);
        public Task<bool> AddToMaterial(int fileId, int materialId);
        public Task<List<FileDto>> GetFiles(int materialId);
        public Task<bool> ArchiveFile(int fileId);
        public Task<List<FileDto>> GetAllFiles();
        public Task<(List<FileDto> files, int totalCount)> GetAllFiles(int page, int pagesize);
        public Task<List<FileDto>> GetAllUserFiles(Guid userId);
        public Task<(List<FileDto> files, int totalCount)> GetAllUserFiles(Guid userId, int page, int pagesize);
        public Task<bool> ChangeFile(FileDto fileDto);
        public Task<(List<FileDto> files, int totalCount)> GetBySearchTag(Guid userId, string question);
        public Task<(List<FileDto> files, int totalCount)> GetBySearchTag(Guid userId, string question, int page, int pageSize);


    }
}
