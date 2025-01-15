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
        public SchoolingPartService(CatchUpDbContext context, IMaterialService materialService)
        {
            _context = context;
            _materialService = materialService;
        }

        public async Task<List<SchoolingPartDto>> GetSchoolingParts(int schoolingId)
        {
            var schoolingParts = await _context.SchoolingParts
            .Where(sp => sp.SchoolingId == schoolingId && sp.State == Enums.StateEnum.Active)
            .Select(sp => new SchoolingPartDto
            {
                Id = sp.Id,
                Name = sp.Name,
                Content = sp.Content
            })
            .ToListAsync();

            foreach (var part in schoolingParts)
            {
                part.Materials = await GetMaterials(part.Id);
            }
            return schoolingParts;
        }

        public async Task<SchoolingPartDto> GetSchoolingPart(int schoolingPartId)
        {
            var result = await _context.SchoolingParts
            .FirstOrDefaultAsync(sp => sp.Id == schoolingPartId && StateEnum.Active == sp.State);
            
            if(result == null)
                return null;

            var schoolingPart = new SchoolingPartDto
            {
                Id = result.Id,
                Name = result.Name,
                Content = result.Content
            };


            schoolingPart.Materials = await GetMaterials(schoolingPartId);
            return schoolingPart;
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

        public async Task<bool> ArchiveSchoolingPart(int schoolingPartId)
        {
            var schoolingPart = await _context.SchoolingParts.FindAsync(schoolingPartId);
            if (schoolingPart == null) return false;

            schoolingPart.State = StateEnum.Archived;

            await _context.MaterialsSchoolingParts
                .Where(msp => msp.SchoolingPartId == schoolingPartId && msp.State == StateEnum.Active)
                .ForEachAsync(msp => msp.State = StateEnum.Archived);
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

        public async Task<bool> ArchiveMaterialFromSchooling(int schoolingPartId, int materialId)
        {
            var msp = await _context.MaterialsSchoolingParts.FindAsync(new object[] { materialId, schoolingPartId });
            if (msp == null) return false;

            msp.State = StateEnum.Archived;
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

        public async Task<List<SchoolingPartDto>> GetAllSchoolingParts()
        {
            var schoolingParts = await _context.SchoolingParts
            .Where(sp => sp.State == Enums.StateEnum.Active)
            .Select(sp => new SchoolingPartDto
            {
                Id = sp.Id,
                Name = sp.Name,
                Content = sp.Content
            })
            .ToListAsync();

            foreach (var part in schoolingParts)
            {
                part.Materials = await GetMaterials(part.Id);
            }
            return schoolingParts;
        }

        public async Task<bool> EditSchoolingPart(SchoolingPartDto schoolingPart)
        {
            if (!await Edit(schoolingPart)) return false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditManySchoolingPart(List<SchoolingPartDto> schoolingParts)
        {
            foreach (var part in schoolingParts)
            {
                if (!await Edit(part)) return false;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> Edit(SchoolingPartDto schoolingPart)
        {
            var schoolingPartModel = await _context.SchoolingParts.FindAsync(schoolingPart.Id);
            if (schoolingPartModel == null) return false;

            schoolingPartModel.Name = schoolingPart.Name;
            schoolingPartModel.Content = schoolingPart.Content;

            var existingMaterials = await _context.MaterialsSchoolingParts
                .Where(m => m.SchoolingPartId == schoolingPart.Id && m.State == StateEnum.Active)
                .Select(m => m.MaterialsId)
                .ToListAsync();

            var materialsToRemove = existingMaterials
                .Where(existingMaterialId => !schoolingPart.Materials.Any(m => m.Id == existingMaterialId))
                .ToList();

            var materialsToAdd = schoolingPart.Materials
                .Where(newMaterial => !existingMaterials.Contains(newMaterial.Id))
                .Select(newMaterial => newMaterial.Id)
                .ToList();

            foreach (var materialId in materialsToRemove)
            {
                await DeleteMaterialFromSchooling(schoolingPart.Id, materialId);
            }

            foreach (var materialId in materialsToAdd)
            {
                await AddMaterialToSchooling(schoolingPart.Id, materialId);
            }
            return true;
        }
    }
}
