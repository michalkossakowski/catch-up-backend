using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CatchUpDbContext _context;
        public CategoryService(CatchUpDbContext context) 
        {
            _context = context;
        }

        public async Task<CategoryDto> GetActiveCategory(int categoryId)
        {
            var categoryModel = await _context.Categories.FindAsync(categoryId)
                ?? throw new NotFoundException("Category not found");
            if (categoryModel.State != Enums.StateEnum.Active)
                throw new NotFoundException("Category not is not active");
            return new CategoryDto { Id = categoryModel.Id, Name = categoryModel.Name };
        }

        public async Task<bool> IsActive(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId && c.State == Enums.StateEnum.Active);
        }

        public async Task<bool> IsExists(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }
    }
}
