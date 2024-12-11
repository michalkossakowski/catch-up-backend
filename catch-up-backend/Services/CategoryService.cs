using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Models;
using catch_up_backend.Interfaces;
using catch_up_backend.Database;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Enums;
using Microsoft.AspNetCore.Components.Web;

namespace catch_up_backend.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CatchUpDbContext _context;

        public CategoryService(CatchUpDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddCategory(CategoryDto newCategory)
        {
            try
            {
                var category = new CategoryModel(newCategory.Name ?? "");
                await _context.AddAsync(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Add category: " + ex);
            }
            return true;
        }

        public async Task<bool> EditCategory(int categoryId, CategoryDto newCategory)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return false;
            }
            try
            {
                category.Name = newCategory.Name;
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Edit category: " + ex);
            }
            return true;
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return false;
            }
            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Delete category: " + ex);
            }
            return true;
        }

        public async Task<CategoryDto> GetById(int categoryId)
        {
            var category = await _context.Categories
                .Where(c => c.Id == categoryId && c.State != StateEnum.Deleted)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).FirstOrDefaultAsync();

            return category;
        }

        public async Task<List<CategoryDto>> GetAll()
        {
            var categories = await _context.Categories
                .Where(c => c.State != StateEnum.Deleted)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync();

            return categories;
        }

        public async Task<List<CategoryDto>> SearchCategories(string searchingString)
        {
            var categories = await _context.Categories
                .Where(c => c.Name.Contains(searchingString) && c.State != StateEnum.Deleted)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync();

            return categories;
        }

        public async Task<bool> IsUnique(string categoryName)
        {
            var category = await _context.Categories
                .Where(c => c.Name == categoryName && c.State != StateEnum.Deleted)
                .FirstOrDefaultAsync();

            return category == null;
        }

        public async Task<int> GetCount()
        {
            return await _context.Categories
                .Where(c => c.State != StateEnum.Deleted)
                .CountAsync();
        }
        public async Task<CategoryDto> GetActiveCategory(int categoryId)
        {
            var categoryModel = await _context.Categories.FindAsync(categoryId);
            if (categoryModel == null || categoryModel.State != Enums.StateEnum.Active)
                return null;
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
