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

                schoolingPartDto = new SchoolingPartDto()
                {
                    Id = part.Id,
                    Title = part.Title,
                    Content = part.Content,
                    ShortDescription = part.ShortDescription,
                    Order = part.Order,
                    MaterialsId = part.MaterialsId
                };
            }
            catch(Exception)
            {
                return null;
            }
            return Task.FromResult(schoolingPartDto);
        }


        public async Task<bool> DeleteSchoolingPart(int schoolingPartId)
        {
            var schoolingPart = await _context.SchoolingParts.FindAsync(schoolingPartId);
            if (schoolingPart == null) return false;

            schoolingPart.State = StateEnum.Deleted;

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
                
                partStatus.Add(
                    new SchoolingPartDto
                    {
                        Id = part.Id,
                        Title = part.Title,
                        ShortDescription = part.ShortDescription,
                        MaterialsId = part.MaterialsId
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
            schoolingPartModel.Order = schoolingPart.Order;
            schoolingPartModel.MaterialsId = schoolingPart.MaterialsId;


            await _context.SaveChangesAsync();
            return true;
        }
    }
}
