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
        public async Task<bool> AddFile(int materialId, int fileId)
        {
            await _fileService.AddToMaterial(fileId, materialId);
            return true;
        }
        public async Task<bool> AddFilesToMaterial(int materialId, List<int> fileIds)
        {
            await _fileService.AddFilesToMaterial(fileIds, materialId);
            return true;
        }
        public async Task<MaterialDto> CreateMaterial(MaterialDto materialDto)
        {
            var material = new MaterialsModel(materialDto.Name);
            await _context.Materials.AddAsync(material);
            await _context.SaveChangesAsync();
            materialDto.Id = material.Id;
            return materialDto;
        }
        public async Task<bool> Delete(int materialId)
        {
            var material = await _context.Materials.FindAsync(materialId);
            if (material == null)
                return false;

            material.State = StateEnum.Deleted;

            var fileInMaterial = _context.FileInMaterials
                .Where(fim => fim.MaterialId == materialId);

            if (fileInMaterial.Any())
            {
                foreach (var fim in fileInMaterial)
                    fim.State = StateEnum.Deleted;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Archive(int materialId)
        {
            var material = await _context.Materials.FindAsync(materialId);
            if (material == null)
                return false;

            material.State = StateEnum.Archived;


            var fileInMaterial = _context.FileInMaterials
                .Where(fim => fim.MaterialId == materialId);

            if (fileInMaterial.Any())
            {
                foreach (var fim in fileInMaterial)
                    fim.State = StateEnum.Archived;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Edit(int materialId, string name)
        {
            var material = await _context.Materials.FindAsync(materialId);
            if (material == null)
                return false;

            material.Name = name;

            _context.Update(material);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<MaterialDto> GetFilesInMaterial(int materialId)
        {
            var material = await _context.Materials.FindAsync(materialId);
            
            if (material == null || material.State != StateEnum.Active)
                return null;

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
            var material = await _context.Materials.FindAsync(materialId);

            if (material == null || material.State != StateEnum.Active)
                return null;

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
        public async Task<bool> RemoveFile(int materialId, int fileId)
        {
            var file = await _fileService.GetById(fileId);
            if(file == null)
                return false;

            var fileInMaterial = await _context.FileInMaterials
                .FirstAsync(fim => fim.MaterialId == materialId && fim.FileId == fileId && fim.State == StateEnum.Active);
            
            if(fileInMaterial == null)
                return false;

            fileInMaterial.State = StateEnum.Archived;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveFilesFromMaterial(int materialId, List<int> fileIds)
        {
            var files = await _fileService.GetFiles(materialId);
            if (files == null || !files.Any())
                return false;

            var filesInMaterial = await _context.FileInMaterials
                .Where(fim => fim.MaterialId == materialId && fileIds.Contains(fim.FileId) && fim.State == StateEnum.Active)
                .ToListAsync();

            if (!filesInMaterial.Any())
                return false;

            foreach (var fim in filesInMaterial)
            {
                fim.State = StateEnum.Archived;
            }

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
