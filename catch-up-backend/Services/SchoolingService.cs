using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using catch_up_backend.Exceptions;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Enums;
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

        public async Task<FullSchoolingDto> CreateSchoolingAsync(SchoolingDto schoolingDto)
        {
            if (schoolingDto.CategoryId == 0)
                throw new ValidationException("Category is neeeded");
            else if(!await _categoryService.IsActive(schoolingDto.CategoryId))
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

        public Task<bool> DeleteAsync(int schoolingId)
        {
            throw new NotImplementedException();
        }


        // Dokończy
        public async Task Edit(FullSchoolingDto fullSchoolingDto)
        {
            var existingSchooling = await _context.Schoolings.FindAsync(fullSchoolingDto.Schooling.Id)
                ?? throw new NotFoundException("Schooling not found");

            existingSchooling.Title = fullSchoolingDto.Schooling.Title;
            existingSchooling.Description = fullSchoolingDto.Schooling.Description;
            existingSchooling.Priority = fullSchoolingDto.Schooling.Priority;
            existingSchooling.CategoryId = fullSchoolingDto.Schooling.CategoryId;
        }

        public async Task<List<FullSchoolingDto>> GetAllFullAsync()
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
        public async Task<FullSchoolingDto> GetFullAsync(int schoolingId)
        {
            var schoolingModel = await _context.Schoolings.FindAsync(schoolingId) 
                ?? throw new NotFoundException("Schooling not found");
            var schooling = new SchoolingDto(schoolingModel);
            
            var category = await _categoryService.GetActiveCategory(schooling.CategoryId);

            var schoolingParts= await _schoolingPartService.GetSchoolingParts(schooling.Id);

            return new FullSchoolingDto(schooling, category, schoolingParts);
        }
    }
}
