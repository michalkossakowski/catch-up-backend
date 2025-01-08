using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;
namespace catch_up_backend.Services
{
    public class SchoolingService : ISchoolingService
    {
        private readonly CatchUpDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly ISchoolingPartService _schoolingPartService;

        public SchoolingService(CatchUpDbContext context, ICategoryService categoryService, ISchoolingPartService schoolingPartService)
        {
            _context = context;
            _categoryService = categoryService;
            _schoolingPartService = schoolingPartService;
        }

        public async Task<SchoolingPartDto> CreateSchoolingPart(SchoolingPartDto schoolingPart, int schoolingID)
        {
            if (schoolingPart == null || schoolingID <= 0)
                return null;

            var schoolingPartModel = new SchoolingPartModel(schoolingID, schoolingPart.Name, schoolingPart.Content);
            _context.SchoolingParts.Add(schoolingPartModel);
            await _context.SaveChangesAsync();
            schoolingPart.Id = schoolingPartModel.Id;

            if (schoolingPart.Materials != null && schoolingPart.Materials.Any())
            {
                foreach (var materialDto in schoolingPart.Materials)
                {
                    var materialSchoolingPart = new MaterialsSchoolingPartModel(materialDto.Id, schoolingPartModel.Id);
                    _context.MaterialsSchoolingParts.Add(materialSchoolingPart);
                }
                await _context.SaveChangesAsync();
            }
            return schoolingPart;
        }

        public async Task<FullSchoolingDto?> CreateSchooling(SchoolingDto schoolingDto)
        {
            if (schoolingDto.CategoryId == 0)
                return null;

            var category = await _categoryService.GetById(schoolingDto.CategoryId);
            if (category == null)
                return null;

            var schooling = new SchoolingModel(
                schoolingDto.CreatorId,
                schoolingDto.CategoryId,
                schoolingDto.Title,
                schoolingDto.Description,
                schoolingDto.Priority
            );
            await _context.AddAsync(schooling);
            await _context.SaveChangesAsync();
            schoolingDto.Id = schooling.Id;

            return new FullSchoolingDto(schoolingDto, category, new List<SchoolingPartDto>());
        }

        public async Task<bool> DeleteSchooling(int schoolingId)
        {
            var schooling = await _context.Schoolings.FindAsync(schoolingId);
            if (schooling == null)
                return false;

            schooling.State = StateEnum.Deleted;

            var schoolingParts = await _context.SchoolingParts
                .Where(sp => sp.SchoolingId == schoolingId)
                .ToListAsync();

            foreach (var schoolingPart in schoolingParts)
            {
                schoolingPart.State = StateEnum.Deleted;

                var materialsSchoolingParts = await _context.MaterialsSchoolingParts
                    .Where(msp => msp.SchoolingPartId == schoolingPart.Id)
                    .ToListAsync();

                foreach (var material in materialsSchoolingParts)
                {
                    material.State = StateEnum.Deleted;
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ArchiveSchooling(int schoolingId)
        {
            var schooling = await _context.Schoolings.FindAsync(schoolingId);
            if (schooling == null)
                return false;

            schooling.State = StateEnum.Archived;

            var schoolingParts = await _context.SchoolingParts
                .Where(sp => sp.SchoolingId == schoolingId)
                .ToListAsync();

            foreach (var schoolingPart in schoolingParts)
            {
                schoolingPart.State = StateEnum.Archived;

                var materialsSchoolingParts = await _context.MaterialsSchoolingParts
                    .Where(msp => msp.SchoolingPartId == schoolingPart.Id)
                    .ToListAsync();

                foreach (var material in materialsSchoolingParts)
                {
                    material.State = StateEnum.Archived;
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Edit(FullSchoolingDto fullSchoolingDto)
        {
            if (fullSchoolingDto == null)
                return false;

            var existingSchooling = await _context.Schoolings.FindAsync(fullSchoolingDto.Schooling.Id);
            if (existingSchooling == null)
                return false;

            await EditSchooling(fullSchoolingDto.Schooling);
            await _schoolingPartService.EditManySchoolingPart(fullSchoolingDto.Parts);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<FullSchoolingDto>> GetAllFull()
        {
            var schoolingList = await _context.Schoolings
                    .Where(s => s.State == Enums.StateEnum.Active)
                    .ToListAsync();

            var fullSchoolingDtos = new List<FullSchoolingDto>();

            foreach (var schoolingModel in schoolingList)
            {
                var schooling = new SchoolingDto(schoolingModel);
                var category = await _categoryService.GetActiveCategory(schooling.CategoryId);
                var schoolingParts = await _schoolingPartService.GetSchoolingParts(schooling.Id);

                fullSchoolingDtos.Add(new FullSchoolingDto(schooling, category, schoolingParts));
            }

            return fullSchoolingDtos;
        }
        public async Task<bool> AddSchoolingToUser(Guid userId, int schoolingId)
        {
            if (await _context.Users.FindAsync(userId) == null)
                return false;

            if (await _context.Schoolings.FindAsync(schoolingId) == null)
                return false;

            var schoolingUser = await _context.SchoolingsUsers
                .FirstOrDefaultAsync(su => su.NewbieId == userId && su.SchoolingId == schoolingId);

            if (schoolingUser == null)
            {
                schoolingUser = new SchoolingUserModel(userId, schoolingId);
                await _context.SchoolingsUsers.AddAsync(schoolingUser);
            }
            else
                schoolingUser.State = StateEnum.Active;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ArchiveUserSchooling(Guid userId, int schoolingId)
        {
            if (await _context.Users.FindAsync(userId) == null)
                return false;

            if (await _context.Schoolings.FindAsync(schoolingId) == null)
                return false;

            var schoolingUser = await _context.SchoolingsUsers
                .FirstOrDefaultAsync(su => su.NewbieId == userId && su.SchoolingId == schoolingId);

            if(schoolingUser == null)
                return false;

            schoolingUser.State = StateEnum.Archived;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserSchooling(Guid userId, int schoolingId)
        {
            if (await _context.Users.FindAsync(userId) == null)
                return false;

            if (await _context.Schoolings.FindAsync(schoolingId) == null)
                return false;

            var schoolingUser = await _context.SchoolingsUsers
                .FirstOrDefaultAsync(su => su.NewbieId == userId && su.SchoolingId == schoolingId);

            if (schoolingUser == null)
                return false;

            schoolingUser.State = StateEnum.Deleted;

            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<List<FullSchoolingDto>> GetAllFull(Guid userId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                return null;

            var schoolingList = await _context.Schoolings
                .Where(schooling => schooling.State == StateEnum.Active)
                .Join(_context.SchoolingsUsers,
                    schooling => schooling.Id,
                    schoolingUser => schoolingUser.SchoolingId,
                    (schooling, schoolingUser) => new { schooling, schoolingUser })
                .Where(joined => joined.schoolingUser.NewbieId == userId)
                .Select(joined => joined.schooling)
                .ToListAsync();
            var fullSchoolingDtos = new List<FullSchoolingDto>();

            foreach (var schoolingModel in schoolingList)
            {
                var schooling = new SchoolingDto(schoolingModel);
                var category = await _categoryService.GetActiveCategory(schooling.CategoryId);
                var schoolingParts = await _schoolingPartService.GetSchoolingParts(schooling.Id);

                fullSchoolingDtos.Add(new FullSchoolingDto(schooling, category, schoolingParts));
            }

            return fullSchoolingDtos;
        }

        public async Task<FullSchoolingDto> GetFull(int schoolingId)
        {
            var schoolingModel = await _context.Schoolings.FindAsync(schoolingId);
            if(schoolingModel == null)
                return null;

            var schooling = new SchoolingDto(schoolingModel);

            var category = await _categoryService.GetActiveCategory(schooling.CategoryId);

            var schoolingParts = await _schoolingPartService.GetSchoolingParts(schooling.Id);

            return new FullSchoolingDto(schooling, category, schoolingParts);
        }

        public async Task<List<int>> GetUserSchoolingsID(Guid userId)
        {
            if (await _context.Users.FindAsync(userId) == null)
                return null;

            return await _context.SchoolingsUsers
                .Where(su => su.NewbieId == userId && su.State == StateEnum.Active)
                .Select(su => su.SchoolingId)
                .ToListAsync();
        }

        public async Task<bool> EditSchooling(SchoolingDto schoolingDto)
        {
            var existingSchooling = await _context.Schoolings.FindAsync(schoolingDto.Id);
            if(existingSchooling == null)
                return false;

            if(!await _categoryService.IsActive(existingSchooling.CategoryId))
                return false;
            existingSchooling.Title = schoolingDto.Title;
            existingSchooling.Description = schoolingDto.Description;
            existingSchooling.Priority = schoolingDto.Priority;
            existingSchooling.CategoryId = schoolingDto.CategoryId;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
