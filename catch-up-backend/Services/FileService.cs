using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.FileManagers;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Services
{
    public class FileService : IFileService
    {
        private readonly CatchUpDbContext _context;
        private readonly IFileStorage _fileStorage;
        public FileService(FileStorageFactory fileStorageFactory, CatchUpDbContext context)
        {
            _context = context;
            _fileStorage = fileStorageFactory.CreateFileStorage();
        }

        public async Task<FileDto> UploadFileAsync(IFormFile newFile, int? materialID)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{newFile.FileName}";

            using Stream fileStream = newFile.OpenReadStream();

            var fileSource = await _fileStorage.UploadFileAsync(uniqueFileName, fileStream);

            FileModel fileModel = new FileModel(newFile.FileName, newFile.ContentType, fileSource);
            
            await _context.Files.AddAsync(fileModel);
            await _context.SaveChangesAsync();

            if (materialID != null)
                await AddToMaterialAsync(fileModel.Id, (int)materialID);

            await _context.SaveChangesAsync();
            FileDto fileDto = new FileDto
            {
                Id = fileModel.Id,
                Name = fileModel.Name,
                Type = fileModel.Type,
                Source = fileModel.Source
            };

            return fileDto;
        }

        public async Task DeleteFileAsync(int fileId)
        {
            var file = await _context.Files.FindAsync(fileId) ?? throw new FileNotFoundException("File not found in database.");
            await _fileStorage.DeleteFileAsync(file.Source);
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
        }

        public async Task<FileDto> GetByIdAsync(int fileId)
        {
            var file = await _context.Files.FindAsync(fileId) ?? throw new FileNotFoundException("File not found in database.");
            return new FileDto { Id = file.Id, Name = file.Name, Type = file.Type };
        }

        public async Task<Stream> DownloadFileAsync(int fileId)
        {
            var file = await _context.Files.FindAsync(fileId) ?? throw new FileNotFoundException("File not found in database.");
            return await _fileStorage.DownloadFileAsync(file.Source);
        }
        public async Task AddToMaterialAsync(int fileId, int materialId)
        {
            var file = await _context.Files.FindAsync(fileId) ?? throw new FileNotFoundException("File not found in database.");
            if (await _context.Materials.FindAsync(materialId) == null)
                throw new Exception("Material not found");
            var connectFileMaterial = new FileInMaterial(materialId, fileId);
            await _context.AddAsync(connectFileMaterial);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FileDto>> GetFilesAsync(int materialId)
        {
            return await _context.FileInMaterials
                .Where(fim => fim.MaterialId == materialId)
                .Join(
                    _context.Files,
                    fim => fim.FileId,
                    file => file.Id,
                    (fim, file) => new FileDto
                    {
                        Id = file.Id,
                        Name = file.Name,
                    })
                .ToListAsync();
        }
    }
}
