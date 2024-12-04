using catch_up_backend.Database;
using catch_up_backend.Dtos;
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

        public async Task<List<MaterialDto>> GetMaterials(int schoolingPartId)
        {
            var materialIds = await _context.MaterialsSchoolingParts
                .Where(msp => msp.SchoolingPartId == schoolingPartId && msp.State == Enums.StateEnum.Active)
                .Select(msp => msp.MaterialsId)
                .ToListAsync();

            var materials = new List<MaterialDto>();

            foreach (var materialId in materialIds)
            {
                materials.Add(await _materialService.GetFilesInMaterial(materialId));
            }
            return materials;
        }


    }
}
