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

        public async Task AddSchoolingPart(SchoolingPartDto schoolingPart, int schoolingID)
        {
            var schoolingPartModel = new SchoolingPartModel(schoolingID, schoolingPart.Name, schoolingPart.Content);
            _context.SchoolingParts.Add(schoolingPartModel);

            await _context.SaveChangesAsync();
            if (schoolingPart.Materials != null && schoolingPart.Materials.Any())
            {
                foreach (var materialDto in schoolingPart.Materials)
                {
                    var materialSchoolingPart = new MaterialsSchoolingPartModel(materialDto.Id, schoolingPartModel.Id);
                    _context.MaterialsSchoolingParts.Add(materialSchoolingPart);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<FullSchoolingDto> CreateSchooling(SchoolingDto schoolingDto)
        {
            if (schoolingDto.CategoryId == 0)
                throw new ValidationException("Category is neeeded");
            else if (!await _categoryService.IsActive(schoolingDto.CategoryId))
                throw new NotFoundException("Category not found or is not active");

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
            return new FullSchoolingDto(schoolingDto, new CategoryDto(), new List<SchoolingPartDto>());
        }

        public async Task DeleteSchooling(int schoolingId)
        {
            var schooling = await _context.Schoolings.FindAsync(schoolingId)
                ?? throw new NotFoundException("Schooling not found");
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
        }

        public async Task ArchiveSchooling(int schoolingId)
        {
            var schooling = await _context.Schoolings.FindAsync(schoolingId)
                ?? throw new NotFoundException("Schooling not found");
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
        }


        // Dokończyć
        public async Task Edit(FullSchoolingDto fullSchoolingDto)
        {
            var existingSchooling = await _context.Schoolings.FindAsync(fullSchoolingDto.Schooling.Id)
                ?? throw new NotFoundException("Schooling not found");

            existingSchooling.Title = fullSchoolingDto.Schooling.Title;
            existingSchooling.Description = fullSchoolingDto.Schooling.Description;
            existingSchooling.Priority = fullSchoolingDto.Schooling.Priority;
            existingSchooling.CategoryId = fullSchoolingDto.Schooling.CategoryId;
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
        public async Task AddSchoolingToUser(Guid userId, int schoolingId)
        {
            if (await _context.Users.FindAsync(userId) == null)
                throw new NotFoundException("User not found in database");

            if (await _context.Schoolings.FindAsync(schoolingId) == null)
                throw new NotFoundException("Schooling not found in database");

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
        }

        public async Task ArchiveUserSchooling(Guid userId, int schoolingId)
        {
            if (await _context.Users.FindAsync(userId) == null)
                throw new NotFoundException("User not found in database");

            if (await _context.Schoolings.FindAsync(schoolingId) == null)
                throw new NotFoundException("Schooling not found in database");

            var schoolingUser = await _context.SchoolingsUsers
                .FirstOrDefaultAsync(su => su.NewbieId == userId && su.SchoolingId == schoolingId)
                ?? throw new NotFoundException("User is not assign to this schooling");

            schoolingUser.State = StateEnum.Archived;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserSchooling(Guid userId, int schoolingId)
        {
            if (await _context.Users.FindAsync(userId) == null)
                throw new NotFoundException("User not found in database");

            if (await _context.Schoolings.FindAsync(schoolingId) == null)
                throw new NotFoundException("Schooling not found in database");

            var schoolingUser = await _context.SchoolingsUsers
                .FirstOrDefaultAsync(su => su.NewbieId == userId && su.SchoolingId == schoolingId)
                ?? throw new NotFoundException("User is not assign to this schooling");

            schoolingUser.State = StateEnum.Deleted;

            await _context.SaveChangesAsync();
        }

        public async Task<List<FullSchoolingDto>> GetAllFull(Guid userId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
            {
                throw new NotFoundException("User not found in database");
            }
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
            var schoolingModel = await _context.Schoolings.FindAsync(schoolingId)
                ?? throw new NotFoundException("Schooling not found");
            var schooling = new SchoolingDto(schoolingModel);

            var category = await _categoryService.GetActiveCategory(schooling.CategoryId);

            var schoolingParts = await _schoolingPartService.GetSchoolingParts(schooling.Id);

            return new FullSchoolingDto(schooling, category, schoolingParts);
        }

        public async Task<List<int>> GetUserSchoolingsID(Guid userId)
        {
            if (await _context.Users.FindAsync(userId) == null)
                throw new NotFoundException("User not found in database");

            return await _context.SchoolingsUsers
                .Where(su => su.NewbieId == userId && su.State == StateEnum.Active)
                .Select(su => su.SchoolingId)
                .ToListAsync();
        }
    }
}
