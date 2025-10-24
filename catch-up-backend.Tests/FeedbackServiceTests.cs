using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using catch_up_backend.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace catch_up_backend.Tests
{
    public class FeedbackServiceTests
    {
        private readonly DbContextOptions<CatchUpDbContext> _options;

        public FeedbackServiceTests()
        {
            _options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task AddAsync_ShouldAddFeedback_WhenValidFeedbackProvided()
        {
            // Arrange
            var feedbackDto = new FeedbackDto
            {
                SenderId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                Title = "Test Feedback",
                Description = "Test Description",
                ResourceType = ResourceTypeEnum.Faq,
                ResourceId = 1,
                MaterialId = null
            };

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new FeedbackService(
                    context,
                    Mock.Of<ISchoolingService>(),
                    Mock.Of<ITaskService>(),
                    Mock.Of<IFaqService>(),
                    Mock.Of<IUserRepository>());

                // Act
                var result = await service.AddAsync(feedbackDto);

                // Assert
                Assert.True(result);
                Assert.Single(context.Feedbacks);
                var feedback = context.Feedbacks.First();
                Assert.Equal("Test Feedback", feedback.Title);
            }
        }

        [Fact]
        public async Task EditAsync_ShouldEditFeedback_WhenFeedbackExists()
        {
            // Arrange
            var feedbackId = 1;
            var existingFeedback = new FeedbackModel
            {
                Id = feedbackId,
                SenderId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                Title = "Old Title",
                Description = "Old Description",
                ResourceType = ResourceTypeEnum.Faq,
                ResourceId = 1,
                State = StateEnum.Active
            };

            using (var context = new CatchUpDbContext(_options))
            {
                context.Feedbacks.Add(existingFeedback);
                await context.SaveChangesAsync();
            }

            var updatedFeedbackDto = new FeedbackDto
            {
                SenderId = existingFeedback.SenderId,
                ReceiverId = existingFeedback.ReceiverId,
                Title = "Updated Title",
                Description = "Updated Description",
                ResourceType = ResourceTypeEnum.Task,
                ResourceId = 2,
                MaterialId = null
            };

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new FeedbackService(
                    context,
                    Mock.Of<ISchoolingService>(),
                    Mock.Of<ITaskService>(),
                    Mock.Of<IFaqService>(),
                    Mock.Of<IUserRepository>());

                // Act
                var result = await service.EditAsync(feedbackId, updatedFeedbackDto);

                // Assert
                Assert.True(result);
                var feedback = context.Feedbacks.First();
                Assert.Equal("Updated Title", feedback.Title);
                Assert.Equal("Updated Description", feedback.Description);
            }
        }

        [Fact]
        public async Task DeleteAsync_ShouldMarkFeedbackAsDeleted_WhenFeedbackExists()
        {
            // Arrange
            var feedbackId = 1;
            var existingFeedback = new FeedbackModel
            {
                Id = feedbackId,
                SenderId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                Title = "Feedback to Delete",
                State = StateEnum.Active,
                Description = "Some Description"
            };

            using (var context = new CatchUpDbContext(_options))
            {
                context.Feedbacks.Add(existingFeedback);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new FeedbackService(
                    context,
                    Mock.Of<ISchoolingService>(),
                    Mock.Of<ITaskService>(),
                    Mock.Of<IFaqService>(),
                    Mock.Of<IUserRepository>());

                // Act
                var result = await service.DeleteAsync(feedbackId);

                // Assert
                Assert.True(result);
                var feedback = context.Feedbacks.First();
                Assert.Equal(StateEnum.Deleted, feedback.State);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnFeedback_WhenFeedbackExists()
        {
            // Arrange
            var feedbackId = 1;
            var existingFeedback = new FeedbackModel
            {
                Id = feedbackId,
                SenderId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                Title = "Existing Feedback",
                State = StateEnum.Active,
                Description = "Some Description"
            };

            using (var context = new CatchUpDbContext(_options))
            {
                context.Feedbacks.Add(existingFeedback);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new FeedbackService(
                    context,
                    Mock.Of<ISchoolingService>(),
                    Mock.Of<ITaskService>(),
                    Mock.Of<IFaqService>(),
                    Mock.Of<IUserRepository>());

                // Act
                var result = await service.GetByIdAsync(feedbackId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Existing Feedback", result.Title);
            }
        }
    }
}
