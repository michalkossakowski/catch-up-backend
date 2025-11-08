using System.Collections.Generic;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Services
{
    public class SchoolingPartService : ISchoolingPartService
    {
        private readonly CatchUpDbContext _context;
        private readonly IMaterialService _materialService;
        private readonly IFileService _fileService;

        public SchoolingPartService(CatchUpDbContext context, IMaterialService materialService, IFileService fileService)
        {
            _context = context;
            _materialService = materialService;
            _fileService = fileService;
        }
        public Task<SchoolingPartDto> GetSchoolingPart(int schoolingPartId)
        {
            SchoolingPartDto schoolingPartDto;
            try
            {
                var part = _context.SchoolingParts
                    .FirstOrDefault(sp => sp.Id == schoolingPartId && sp.State == Enums.StateEnum.Active);
                if (part == null) return null;
                FileDto fileDto = null;
                if (!(part.IconFileId is null))
                    fileDto = _fileService.GetById((int)part.IconFileId).Result;
                schoolingPartDto = new SchoolingPartDto()
                {
                    Id = part.Id,
                    Title = part.Title,
                    Content = part.Content,
                    ShortDescription = part.ShortDescription,
                    IconFile = fileDto,
                    Order = part.Order,
                    MaterialsId = _context.MaterialsSchoolingParts
                        .Where(msp => msp.SchoolingPartId == schoolingPartId && msp.State == Enums.StateEnum.Active)
                        .Select(msp => msp.MaterialsId)
                        .ToList()
                };
            }
            catch(Exception)
            {
                return null;
            }
            return Task.FromResult(schoolingPartDto);
        }
        public async Task<List<MaterialDto>> GetMaterials(int schoolingPartId)
        {
            var materialIds = await _context.MaterialsSchoolingParts
                .Where(msp => msp.SchoolingPartId == schoolingPartId && msp.State == Enums.StateEnum.Active)
                .Select(msp => msp.MaterialsId)
                .ToListAsync();

            var materials = new List<MaterialDto>();

            materialIds = await _context.Materials
                .Where(m => materialIds.Contains(m.Id) && m.State == Enums.StateEnum.Active)
                .Select(m => m.Id)
                .ToListAsync();

            foreach (var materialId in materialIds)
            {
                materials.Add(await _materialService.GetFilesInMaterial(materialId));
            }
            return materials;
        }

        public async Task<bool> AddMaterialToSchooling(int schoolingPartId, int materialId)
        {
            if (!await _context.Materials.AnyAsync(m => m.Id == materialId && m.State == StateEnum.Active) ||
                !await _context.SchoolingParts.AnyAsync(sp => sp.Id == schoolingPartId && sp.State == StateEnum.Active))
            {
                return false;
            }

            var materialInSchooling = await _context.MaterialsSchoolingParts.FindAsync(new object[] { materialId, schoolingPartId });

            if (materialInSchooling == null)
            {
                materialInSchooling = new MaterialsSchoolingPartModel(materialId, schoolingPartId);
                await _context.AddAsync(materialInSchooling);
            }
            else
                materialInSchooling.State = StateEnum.Active;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSchoolingPart(int schoolingPartId)
        {
            var schoolingPart = await _context.SchoolingParts.FindAsync(schoolingPartId);
            if (schoolingPart == null) return false;

            schoolingPart.State = StateEnum.Deleted;

            await _context.MaterialsSchoolingParts
                .Where(msp => msp.SchoolingPartId == schoolingPartId)
                .ForEachAsync(msp => msp.State = StateEnum.Archived);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMaterialFromSchooling(int schoolingPartId, int materialId)
        {
            var msp = await _context.MaterialsSchoolingParts.FindAsync(new object[] { materialId, schoolingPartId });
            if (msp == null) return false;

            msp.State = StateEnum.Deleted;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SchoolingPartDto>> GetSchoolingParts(int schoolingId)
        {
            var parts = await _context.SchoolingParts
                .Where(sp => sp.SchoolingId == schoolingId && sp.State == Enums.StateEnum.Active)
                .ToListAsync();
            if (parts == null) return null;

            List<SchoolingPartDto> partStatus = new();
            foreach (var part in parts)
            {
                FileDto fileDto = null;

                if (!(part.IconFileId is null))
                    fileDto = await _fileService.GetById((int)part.IconFileId);

                partStatus.Add(
                    new SchoolingPartDto
                    {
                        Id = part.Id,
                        Title = part.Title,
                        IconFile = fileDto,
                        ShortDescription = part.ShortDescription
                    }
                );
            }
            return partStatus.OrderBy(x => x.Order).ToList();
        }


        public async Task<bool> EditSchoolingPart(SchoolingPartDto schoolingPart)
        {
            var schoolingPartModel = await _context.SchoolingParts.FindAsync(schoolingPart.Id);
            if (schoolingPartModel is null) return false;

            schoolingPartModel.Title = schoolingPart.Title;
            schoolingPartModel.Content = schoolingPart.Content;
            schoolingPartModel.ShortDescription = schoolingPart.ShortDescription;
            schoolingPartModel.IconFileId = schoolingPart.Id;
            
            var existingMaterials = await _context.MaterialsSchoolingParts
              .Where(m => m.SchoolingPartId == schoolingPart.Id && m.State == StateEnum.Active)
              .Select(m => m.MaterialsId)
              .ToListAsync();

            var materialsToRemove = existingMaterials
                .Where(existingMaterialId => !schoolingPart.MaterialsId.Any(id => id == existingMaterialId))
                .ToList();

            var materialsToAdd = schoolingPart.MaterialsId
                .Where(id => !existingMaterials.Contains(id))
                .ToList();

            foreach (var materialId in materialsToRemove)
            {
                await DeleteMaterialFromSchooling(schoolingPart.Id, materialId);
            }

            foreach (var materialId in materialsToAdd)
            {
                await AddMaterialToSchooling(schoolingPart.Id, materialId);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> EditManySchoolingPart(List<SchoolingPartDto> schoolingPart)
        {
            throw new NotImplementedException();
        }
    }
}
