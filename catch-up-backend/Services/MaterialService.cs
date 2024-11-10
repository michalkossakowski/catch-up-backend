using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly CatchUpDbContext _context;
        private readonly IFileService _fileService;
        public MaterialService(CatchUpDbContext context, IFileService fileService) 
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task AddFileAsync(int materialId, int fileId)
        { 
            await _fileService.AddToMaterialAsync(fileId, materialId);
            var addedFile = await _fileService.GetByIdAsync(fileId);
        }

        public async Task<MaterialDto> CreateMaterialAsync(MaterialDto materialDto)
        {
            var material = new MaterialsModel(materialDto.Name);
            await _context.Materials.AddAsync(material);
            await _context.SaveChangesAsync();
            return materialDto;
        }

        public async Task DeleteAsync(int materialId)
        {
            var fileInMaterial =  _context.FileInMaterials
                .Where(fim => fim.MaterialId == materialId);
            if (fileInMaterial.Any()) 
                _context.FileInMaterials.RemoveRange(fileInMaterial);

            var material = await _context.Materials.FindAsync(materialId) ?? throw new Exception("Material not found");
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(int materialId, string name)
        {
            var material = await _context.Materials.FindAsync(materialId) ?? throw new Exception("Material not found");
            material.Name = name;
            _context.Update(material);
            await _context.SaveChangesAsync();
        }

        public async Task<MaterialDto> GetFilesInMaterialAsync(int materialId)
        {
            var files = await _fileService.GetFilesAsync(materialId);
            var material = await _context.Materials.FindAsync(materialId);

            var FilesInMaterial = new MaterialDto
            {
                Id = materialId,
                Name = material.Name,
                Files = files
            };
            return FilesInMaterial;
        }

        public async Task<MaterialDto> GetMaterialAsync(int materialId)
        {

            var material = await _context.Materials.FindAsync(materialId);
            var materialDto = new MaterialDto { Id = material.Id, Name = material.Name };
            return materialDto;
        }

        public async Task<List<MaterialDto>> GetMaterialsAync()
        {
            var materials = await _context.Materials.Select(m => new MaterialDto 
            { 
                Id = m.Id,
                Name = m.Name
            }).ToListAsync();
            return materials;
        }
        public async Task RemoveFileAsync(int materialId, int fileId)
        {
            var file = await _fileService.GetByIdAsync(fileId);
            var fileInMaterial = await _context.FileInMaterials
                .FirstAsync(fim => fim.MaterialId == materialId && fim.FileId == fileId);
            _context.FileInMaterials.Remove(fileInMaterial);
            await _context.SaveChangesAsync();
        }
    }
}
