using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Models;
using catch_up_backend.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace catch_up_backend.Tests
{
    public class CategoryServiceTests
    {
        private readonly DbContextOptions<CatchUpDbContext> _options;

        public CategoryServiceTests()
        {
            _options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task AddCategory_ShouldAddCategory_WhenValidCategoryDtoProvided()
        {
            // Arrange
            var newCategory = new CategoryDto { Name = "Test Category" };

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new CategoryService(context);

                // Act
                var result = await service.AddCategory(newCategory);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Test Category", result.Name);
                Assert.Single(context.Categories);
                Assert.Equal("Test Category", context.Categories.First().Name);
            }
        }

        [Fact]
        public async Task EditCategory_ShouldUpdateCategory_WhenCategoryExists()
        {
            // Arrange
            var existingCategory = new CategoryModel("Old Name") { Id = 1 };
            using (var context = new CatchUpDbContext(_options))
            {
                context.Categories.Add(existingCategory);
                await context.SaveChangesAsync();
            }

            var updatedCategory = new CategoryDto { Name = "Updated Name" };

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new CategoryService(context);

                // Act
                var result = await service.EditCategory(1, updatedCategory);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Updated Name", result.Name);
                Assert.Equal("Updated Name", context.Categories.First().Name);
            }
        }

        [Fact]
        public async Task DeleteCategory_ShouldRemoveCategory_WhenCategoryExists()
        {
            // Arrange
            var existingCategory = new CategoryModel("Test Category") { Id = 1 };
            using (var context = new CatchUpDbContext(_options))
            {
                context.Categories.Add(existingCategory);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new CategoryService(context);

                // Act
                var result = await service.DeleteCategory(1);

                // Assert
                Assert.True(result);
                Assert.Empty(context.Categories);
            }
        }

        [Fact]
        public async Task GetById_ShouldReturnCategory_WhenCategoryExists()
        {
            // Arrange
            var existingCategory = new CategoryModel("Test Category") { Id = 1, State = StateEnum.Active };
            using (var context = new CatchUpDbContext(_options))
            {
                context.Categories.Add(existingCategory);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new CategoryService(context);

                // Act
                var result = await service.GetById(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal("Test Category", result.Name);
            }
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllCategories_WhenCategoriesExist()
        {
            // Arrange
            var categories = new List<CategoryModel>
            {
                new CategoryModel("Category 1") { Id = 1, State = StateEnum.Active },
                new CategoryModel("Category 2") { Id = 2, State = StateEnum.Active }
            };

            using (var context = new CatchUpDbContext(_options))
            {
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new CategoryService(context);

                // Act
                var result = await service.GetAll();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.Contains(result, c => c.Name == "Category 1");
                Assert.Contains(result, c => c.Name == "Category 2");
            }
        }

        [Fact]
        public async Task IsUnique_ShouldReturnTrue_WhenCategoryNameIsUnique()
        {
            // Arrange
            var existingCategory = new CategoryModel("Existing Category") { Id = 1, State = StateEnum.Active };
            using (var context = new CatchUpDbContext(_options))
            {
                context.Categories.Add(existingCategory);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new CategoryService(context);

                // Act
                var result = await service.IsUnique("Unique Category");

                // Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async Task IsUnique_ShouldReturnFalse_WhenCategoryNameIsNotUnique()
        {
            // Arrange
            var existingCategory = new CategoryModel("Existing Category") { Id = 1, State = StateEnum.Active };
            using (var context = new CatchUpDbContext(_options))
            {
                context.Categories.Add(existingCategory);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new CategoryService(context);

                // Act
                var result = await service.IsUnique("Existing Category");

                // Assert
                Assert.False(result);
            }
        }
    }
}
