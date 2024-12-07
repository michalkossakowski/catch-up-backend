using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
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
        public async Task AddFile(int materialId, int fileId)
        {
            await _fileService.AddToMaterial(fileId, materialId);
            var addedFile = await _fileService.GetById(fileId);
        }

        public async Task<MaterialDto> CreateMaterial(MaterialDto materialDto)
        {
            var material = new MaterialsModel(materialDto.Name);
            await _context.Materials.AddAsync(material);
            await _context.SaveChangesAsync();
            materialDto.Id = material.Id;
            return materialDto;
        }

        public async Task Delete(int materialId)
        {
            var material = await _context.Materials.FindAsync(materialId)
                ?? throw new NotFoundException("Material not found");
            material.State = StateEnum.Deleted;

            var fileInMaterial = _context.FileInMaterials
                .Where(fim => fim.MaterialId == materialId);

            if (fileInMaterial.Any())
            {
                foreach (var fim in fileInMaterial)
                    fim.State = StateEnum.Deleted;
            }

            await _context.SaveChangesAsync();
        }
        public async Task Archive(int materialId)
        {
            var material = await _context.Materials.FindAsync(materialId)
                ?? throw new NotFoundException("Material not found");

            material.State = StateEnum.Archived;


            var fileInMaterial = _context.FileInMaterials
                .Where(fim => fim.MaterialId == materialId);

            if (fileInMaterial.Any())
            {
                foreach (var fim in fileInMaterial)
                    fim.State = StateEnum.Archived;
            }

            await _context.SaveChangesAsync();
        }

        public async Task Edit(int materialId, string name)
        {
            var material = await _context.Materials.FindAsync(materialId)
                ?? throw new NotFoundException("Material not found");

            material.Name = name;

            _context.Update(material);
            await _context.SaveChangesAsync();
        }

        public async Task<MaterialDto> GetFilesInMaterial(int materialId)
        {
            var material = await _context.Materials.FindAsync(materialId)
                ?? throw new NotFoundException("Material not found");

            if (material.State != StateEnum.Active)
                throw new NotFoundException("Material not active");
            var files = await _fileService.GetFiles(materialId);

            var FilesInMaterial = new MaterialDto
            {
                Id = materialId,
                Name = material.Name,
                Files = files
            };

            return FilesInMaterial;
        }

        public async Task<MaterialDto> GetMaterial(int materialId)
        {
            var material = await _context.Materials.FindAsync(materialId)
                ?? throw new NotFoundException("Material not found");

            if (material.State != StateEnum.Active)
                throw new NotFoundException("Material not found.");

            var materialDto = new MaterialDto { Id = material.Id, Name = material.Name };

            return materialDto;
        }

        public async Task<List<MaterialDto>> GetMaterials()
        {
            var materials = await _context.Materials.Where(m => m.State == StateEnum.Active).Select(m => new MaterialDto
            {
                Id = m.Id,
                Name = m.Name
            }).ToListAsync();
            return materials;
        }
        public async Task RemoveFile(int materialId, int fileId)
        {
            var file = await _fileService.GetById(fileId);
            var fileInMaterial = await _context.FileInMaterials
                .FirstAsync(fim => fim.MaterialId == materialId && fim.FileId == fileId && fim.State == StateEnum.Active);
            fileInMaterial.State = StateEnum.Archived;
            await _context.SaveChangesAsync();
        }
    }
}
