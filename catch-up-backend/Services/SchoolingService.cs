using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using catch_up_backend.Exceptions;
namespace catch_up_backend.Services
{
    public class SchoolingService : ISchoolingService
    {
        private readonly CatchUpDbContext _context;
        private readonly ICategoryService _categoryService;
        public SchoolingService(CatchUpDbContext context, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
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

        public Task<List<FullSchoolingDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<FullSchoolingDto> GetFullAsync(int schoolingId)
        {
            var schooling = await _context.Schoolings.FindAsync(schoolingId) 
                ?? throw new NotFoundException("Schooling not found");
            var schoolingDto = new SchoolingDto(schooling);
            return new FullSchoolingDto(schoolingDto, new CategoryDto(), new List<SchoolingPartDto>() );
        }
    }
}
