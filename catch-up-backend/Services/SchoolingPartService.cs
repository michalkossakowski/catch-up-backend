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

/*        public async Task<List<SchoolingPartDto>> GetSchoolingParts(int schoolingId)
        {
            var schoolingParts = await _context.SchoolingParts
            .Where(sp => sp.SchoolingId == schoolingId && sp.State == Enums.StateEnum.Active)
            .Select(sp => new SchoolingPartDto
            {
                Id = sp.Id,
                Title = sp.Title,
                Content = sp.Content
            })
            .ToListAsync();

            foreach (var part in schoolingParts)
            {
                part.Materials = await GetMaterials(part.Id);
            }
            return schoolingParts;
        }*/
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

/*        public async Task<List<SchoolingPartDto>> GetAllSchoolingParts()
        {
            var schoolingParts = await _context.SchoolingParts
            .Where(sp => sp.State == Enums.StateEnum.Active)
            .Select(sp => new SchoolingPartDto
            {
                Id = sp.Id,
                Title = sp.Title,
                Content = sp.Content
            })
            .ToListAsync();

            foreach (var part in schoolingParts)
            {
                part.Materials = await GetMaterials(part.Id);
            }
            return schoolingParts;
        }*/
/*
        public async Task<bool> EditManySchoolingPart(List<SchoolingPartDto> schoolingParts)
        {
            foreach (var part in schoolingParts)
            {
                if (!await Edit(part)) return false;
            }
            await _context.SaveChangesAsync();
            return true;
        }*/

/*        private async Task<bool> Edit(SchoolingPartDto schoolingPart)
        {
            var schoolingPartModel = await _context.SchoolingParts.FindAsync(schoolingPart.Id);
            if (schoolingPartModel == null) return false;

            schoolingPartModel.Title = schoolingPart.Title;
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
        }*/

        //Done
        public async Task<List<SchoolingPartProgressBarDto>> GetSchoolingPartStatus(int schoolingId)
        {
            var parts = await _context.SchoolingParts
                .Where(sp => sp.SchoolingId == schoolingId && sp.State == Enums.StateEnum.Active)
                .ToListAsync();
            if (parts == null) return null;

            List<SchoolingPartProgressBarDto> partStatus = new();
            foreach (var part in parts)
            {
                FileDto fileDto = null;

                if (!(part.IconFileId is null))
                    fileDto = await _fileService.GetById((int)part.IconFileId);

                partStatus.Add(
                    new SchoolingPartProgressBarDto
                    {
                        Id = part.Id,
                        Order = part.Order,
                        Title = part.Title,
                        FileIcon = fileDto,
                        IsDone = false,
                        ShortDescription = part.ShortDescription
                    }
                );
            }
            return partStatus.OrderBy(x => x.Order).ToList();
        }
        public async Task<List<SchoolingPartProgressBarDto>> GetUserSchoolingPartStatus(int schoolingId, Guid userId)
        {
            List<SchoolingPartProgressBarDto> partStatus = await GetSchoolingPartStatus(schoolingId);
            
            int schoolingUserId = await _context.SchoolingsUsers
                .Where(su => su.NewbieId == userId && su.SchoolingId == schoolingId)
                .Select(su => su.Id)
                .FirstOrDefaultAsync();

            var parts = await _context.SchoolingUserParts
                .Where(sps => sps.SchoolingUserId == schoolingUserId)
                .ToListAsync();

            if (parts == null) return null;

            foreach (var part in parts)
            {
                partStatus.First( ps => ps.Id == part.SchoolingPartId ).IsDone = part.IsDone;
            }
            return partStatus;
        }
        public async Task<SchoolingPartDto> GetSchoolingPart(int schoolingPartId, Guid userId)
        {
            var result = await _context.SchoolingParts
            .FirstOrDefaultAsync(sp => sp.Id == schoolingPartId && StateEnum.Active == sp.State);

            if (result == null)
                return null;

            var schoolingPart = new SchoolingPartDto
            {
                Id = result.Id,
                Title = result.Title,
                Content = result.Content,
                ShortDescription = result.ShortDescription,
                IconFile = result.IconFileId != null ? await _fileService.GetById((int)result.IconFileId) : null,
                schoolingUserId = await _context.SchoolingsUsers
                    .Where(sup => sup.SchoolingId == result.SchoolingId && sup.NewbieId == userId)
                    .Select(sup => sup.Id)
                    .FirstOrDefaultAsync()
            };


            schoolingPart.Materials = await _context.MaterialsSchoolingParts.Where(msp => msp.SchoolingPartId == schoolingPartId && msp.State == StateEnum.Active)
                .Select(msp => msp.MaterialsId)
                .ToListAsync();

            return schoolingPart;
        }

        public async Task<bool> ChangeUserSchoolingPartState(int schoolingUserId, int schoolingPartId, bool state)
        {
            var part = await _context.SchoolingUserParts.FindAsync(schoolingUserId, schoolingPartId);
            if (part == null) return false;
            part.IsDone = state;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditSchoolingPart(SchoolingPartUpdateDto schoolingPart)
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

        public Task<List<SchoolingPartDto>> GetSchoolingParts(int schoolingId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SchoolingPartDto>> GetAllSchoolingParts()
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditManySchoolingPart(List<SchoolingPartDto> schoolingPart)
        {
            throw new NotImplementedException();
        }
    }
}
