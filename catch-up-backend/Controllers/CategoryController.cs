using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] CategoryDto newCategory)
        {
            return await _categoryService.AddCategory(newCategory)
                ? Ok(new { message = "Category added", category = newCategory })
                : StatusCode(500, new { message = "Error: Category add" });
        }

        [HttpPut]
        [Route("Edit/{categoryId:int}")]
        public async Task<IActionResult> Edit(int categoryId, [FromBody] CategoryDto newCategory)
        {
            return await _categoryService.EditCategory(categoryId, newCategory)
                ? Ok(new { message = "Category edited", category = newCategory })
                : StatusCode(500, new { message = "Error: Category edit" });
        }

        [HttpDelete]
        [Route("Delete/{categoryId:int}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            return await _categoryService.DeleteCategory(categoryId)
                ? Ok(new { message = "Category deleted", category = categoryId })
                : NotFound(new { message = "Error: Category delete", category = categoryId });
        }

        [HttpGet]
        [Route("GetById/{categoryId}")]
        public async Task<IActionResult> GetById(int categoryId)
        {
            var category = await _categoryService.GetById(categoryId);
            if (category == null)
                return NotFound(new { message = $"Category with id: {categoryId} not found" });
            return Ok(category);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var category = await _categoryService.GetAll();
            return Ok(category);
        }

        [HttpGet]
        [Route("Search/{searchingString}")]
        public async Task<IActionResult> SearchCategories(string searchingString)
        {
            var category = await _categoryService.SearchCategories(searchingString);
            return Ok(category);
        }

        [HttpGet]
        [Route("IsUnique/{categoryName}")]
        public async Task<IActionResult> IsUnique(string categoryName)
        {
            var isUnique = await _categoryService.IsUnique(categoryName);
            return Ok(isUnique);
        }

        [HttpGet]
        [Route("GetCount")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _categoryService.GetCount();
            return Ok(count);
        }
    }
}
