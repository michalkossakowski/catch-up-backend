using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface ICategoryService
    {
        public Task<bool> IsExists(int categoryId);
        public Task<CategoryDto> GetActiveCategory(int categoryId);
        public Task<bool> IsActive(int categoryId);
    }
}
