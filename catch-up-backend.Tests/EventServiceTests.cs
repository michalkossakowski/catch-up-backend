using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catch_up_backend.Constants;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using catch_up_backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace catch_up_backend.Tests
{
    public class EventServiceTests
    {
        private readonly DbContextOptions<CatchUpDbContext> _options;

        public EventServiceTests()
        {
            _options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetUserEvents_ShouldReturnActiveEvents_WhenUserIsNotAdmin()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userRole = UserType.Newbie;

            var events = new List<EventModel>
            {
                new EventModel
                {
                    Id = 1,
                    Title = "Event 1",
                    Description = "Description 1",
                    StartDate = DateTime.Now.AddDays(1),
                    EndDate = DateTime.Now.AddDays(2),
                    OwnerId = userId,
                    TargetUserType = UserType.Newbie,
                    State = StateEnum.Active
                },
                new EventModel
                {
                    Id = 2,
                    Title = "Event 2",
                    Description = "Description 2",
                    StartDate = DateTime.Now.AddDays(1),
                    EndDate = DateTime.Now.AddDays(2),
                    OwnerId = Guid.NewGuid(),
                    TargetUserType = UserType.Admin,
                    State = StateEnum.Active
                }
            };

            using (var context = new CatchUpDbContext(_options))
            {
                context.Events.AddRange(events);
                await context.SaveChangesAsync();
            }

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.GetRole(userId)).ReturnsAsync(userRole);

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new EventService(
                    context,
                    Mock.Of<IEmailService>(),
                    Mock.Of<INotificationService>(),
                    mockUserService.Object,
                    Mock.Of<IServiceProvider>(),
                    Mock.Of<ILogger<EventService>>());

                // Act
                var result = await service.GetUserEvents(userId);

                // Assert
                Assert.Single(result);
                Assert.Contains(result, e => e.Title == "Event 1");
            }
        }

        [Fact]
        public async Task AddAsync_ShouldAddEvent_WhenValidEventDtoProvided()
        {
            // Arrange
            var eventDto = new EventDto
            {
                Title = "New Event",
                Description = "Event Description",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2),
                OwnerId = Guid.NewGuid(),
                TargetUserType = UserType.Newbie
            };

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new EventService(
                    context,
                    Mock.Of<IEmailService>(),
                    Mock.Of<INotificationService>(),
                    Mock.Of<IUserService>(),
                    Mock.Of<IServiceProvider>(),
                    Mock.Of<ILogger<EventService>>());

                // Act
                var result = await service.AddAsync(eventDto);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("New Event", result.Title);
                Assert.Single(context.Events);
                Assert.Equal("New Event", context.Events.First().Title);
            }
        }

        [Fact]
        public async Task DeleteAsync_ShouldMarkEventAsDeleted_WhenEventExistsAndUserIsOwner()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var eventId = 1;

            var existingEvent = new EventModel
            {
                Id = eventId,
                Title = "Event to Delete",
                Description = "Description",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2),
                OwnerId = userId,
                State = StateEnum.Active,
                TargetUserType = UserType.Newbie
            };

            using (var context = new CatchUpDbContext(_options))
            {
                context.Events.Add(existingEvent);
                await context.SaveChangesAsync();
            }

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.GetRole(userId)).ReturnsAsync(UserType.Admin);

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new EventService(
                    context,
                    Mock.Of<IEmailService>(),
                    Mock.Of<INotificationService>(),
                    mockUserService.Object,
                    Mock.Of<IServiceProvider>(),
                    Mock.Of<ILogger<EventService>>());

                // Act
                var result = await service.DeleteAsync(userId, eventId);

                // Assert
                Assert.True(result);
                var deletedEvent = context.Events.First(e => e.Id == eventId);
                Assert.Equal(StateEnum.Deleted, deletedEvent.State);
            }
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenEventDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var eventId = 1;

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new EventService(
                    context,
                    Mock.Of<IEmailService>(),
                    Mock.Of<INotificationService>(),
                    Mock.Of<IUserService>(),
                    Mock.Of<IServiceProvider>(),
                    Mock.Of<ILogger<EventService>>());

                // Act
                var result = await service.DeleteAsync(userId, eventId);

                // Assert
                Assert.False(result);
            }
        }
    }
}
