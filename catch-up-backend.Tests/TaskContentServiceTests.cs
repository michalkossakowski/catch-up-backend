using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using catch_up_backend.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace catch_up_backend.Tests
{
    public class TaskContentServiceTests
    {
        private readonly DbContextOptions<CatchUpDbContext> _options;

        public TaskContentServiceTests()
        {
            _options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Add_ShouldAddTaskContent_WhenValidDataProvided()
        {
            // Arrange
            var newTaskContent = new TaskContentDto
            {
                CreatorId = Guid.NewGuid(),
                CategoryId = 1,
                MaterialsId = 2,
                Title = "Test Task",
                Description = "Test Description"
            };

            var mockBadgeService = new Mock<IBadgeService>();

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new TaskContentService(context, mockBadgeService.Object);

                // Act
                var result = await service.Add(newTaskContent);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Test Task", result.Title);
                Assert.Single(context.TaskContents);
            }
        }

        [Fact]
        public async Task Edit_ShouldUpdateTaskContent_WhenTaskContentExists()
        {
            // Arrange
            var taskContent = new TaskContentModel(Guid.NewGuid(), 1, 2, "Old Title", "Old Description");
            using (var context = new CatchUpDbContext(_options))
            {
                context.TaskContents.Add(taskContent);
                await context.SaveChangesAsync();
            }

            var updatedTaskContent = new TaskContentDto
            {
                CreatorId = taskContent.CreatorId,
                CategoryId = 3,
                MaterialsId = 4,
                Title = "Updated Title",
                Description = "Updated Description"
            };

            var mockBadgeService = new Mock<IBadgeService>();

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new TaskContentService(context, mockBadgeService.Object);

                // Act
                var result = await service.Edit(taskContent.Id, updatedTaskContent);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Updated Title", result.Title);
                Assert.Equal(3, result.CategoryId);
            }
        }

        [Fact]
        public async Task Delete_ShouldMarkTaskContentAsDeleted_WhenTaskContentExists()
        {
            // Arrange
            var taskContent = new TaskContentModel(Guid.NewGuid(), 1, 2, "Task to Delete", "Description");
            using (var context = new CatchUpDbContext(_options))
            {
                context.TaskContents.Add(taskContent);
                await context.SaveChangesAsync();
            }

            var mockBadgeService = new Mock<IBadgeService>();

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new TaskContentService(context, mockBadgeService.Object);

                // Act
                var result = await service.Delete(taskContent.Id);

                // Assert
                Assert.True(result);
                var deletedTaskContent = context.TaskContents.First();
                Assert.Equal(StateEnum.Deleted, deletedTaskContent.State);
            }
        }

        [Fact]
        public async Task GetAll_ShouldReturnPagedTaskContents_WhenCalled()
        {
            // Arrange
            using (var context = new CatchUpDbContext(_options))
            {
                context.TaskContents.AddRange(
                    new TaskContentModel(Guid.NewGuid(), 1, 2, "Task 1", "Description 1"),
                    new TaskContentModel(Guid.NewGuid(), 1, 2, "Task 2", "Description 2")
                );
                await context.SaveChangesAsync();
            }

            var mockBadgeService = new Mock<IBadgeService>();

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new TaskContentService(context, mockBadgeService.Object);

                // Act
                var (taskContents, totalCount) = await service.GetAll(1, 10);

                // Assert
                Assert.Equal(2, totalCount);
                Assert.Equal(2, taskContents.Count);
            }
        }

        [Fact]
        public async Task GetById_ShouldReturnTaskContent_WhenTaskContentExists()
        {
            // Arrange
            var taskContent = new TaskContentModel(Guid.NewGuid(), 1, 2, "Task to Find", "Description");
            using (var context = new CatchUpDbContext(_options))
            {
                context.TaskContents.Add(taskContent);
                await context.SaveChangesAsync();
            }

            var mockBadgeService = new Mock<IBadgeService>();

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new TaskContentService(context, mockBadgeService.Object);

                // Act
                var result = await service.GetById(taskContent.Id);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Task to Find", result.Title);
            }
        }
    }
}
