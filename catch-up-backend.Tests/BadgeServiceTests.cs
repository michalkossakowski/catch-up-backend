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
    public class BadgeServiceTests
    {
        private readonly DbContextOptions<CatchUpDbContext> _options;

        public BadgeServiceTests()
        {
            _options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Add_ShouldAddBadge_WhenValidBadgeDtoProvided()
        {
            // Arrange
            var badgeDto = new BadgeDto
            {
                Name = "Test Badge",
                Description = "Test Description",
                IconId = 1,
                Count = 10,
                CountType = BadgeTypeCountEnum.NewbiesCount
            };

            using (var context = new CatchUpDbContext(_options))
            {
                var badgeService = new BadgeService(context, null);

                // Act
                var result = await badgeService.Add(badgeDto);

                // Assert
                Assert.True(result);
                Assert.Single(context.Badges);
                var badge = context.Badges.First();
                Assert.Equal("Test Badge", badge.Name);
                Assert.Equal("Test Description", badge.Description);
            }
        }

        [Fact]
        public async Task Edit_ShouldUpdateBadge_WhenBadgeExists()
        {
            // Arrange
            var badgeId = 1;
            var existingBadge = new BadgeModel("Old Name", "Old Description", 1, 5, BadgeTypeCountEnum.NewbiesCount)
            {
                Id = badgeId
            };

            var badgeDto = new BadgeDto
            {
                Name = "Updated Name",
                Description = "Updated Description",
                IconId = 2,
                Count = 15,
                CountType = BadgeTypeCountEnum.AssignedTasksCount
            };

            using (var context = new CatchUpDbContext(_options))
            {
                context.Badges.Add(existingBadge);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(_options))
            {
                var badgeService = new BadgeService(context, null);

                // Act
                var result = await badgeService.Edit(badgeId, badgeDto);

                // Assert
                Assert.True(result);
                var badge = context.Badges.First();
                Assert.Equal("Updated Name", badge.Name);
                Assert.Equal("Updated Description", badge.Description);
            }
        }

        [Fact]
        public async Task Delete_ShouldMarkBadgeAsDeleted_WhenBadgeExists()
        {
            // Arrange
            var badgeId = 1;
            var existingBadge = new BadgeModel("Test Badge", "Test Description", 1, 10, BadgeTypeCountEnum.NewbiesCount)
            {
                Id = badgeId,
                State = StateEnum.Active
            };

            using (var context = new CatchUpDbContext(_options))
            {
                context.Badges.Add(existingBadge);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(_options))
            {
                var badgeService = new BadgeService(context, null);

                // Act
                var result = await badgeService.Delete(badgeId);

                // Assert
                Assert.True(result);
                var badge = context.Badges.First();
                Assert.Equal(StateEnum.Deleted, badge.State);
            }
        }

        [Fact]
        public async Task GetById_ShouldReturnBadgeDto_WhenBadgeExists()
        {
            // Arrange
            var badgeId = 1;
            var existingBadge = new BadgeModel("Test Badge", "Test Description", 1, 10, BadgeTypeCountEnum.NewbiesCount)
            {
                Id = badgeId,
                State = StateEnum.Active
            };

            using (var context = new CatchUpDbContext(_options))
            {
                context.Badges.Add(existingBadge);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(_options))
            {
                var badgeService = new BadgeService(context, null);

                // Act
                var result = await badgeService.GetById(badgeId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(badgeId, result.Id);
                Assert.Equal("Test Badge", result.Name);
            }
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfBadgeDtos_WhenBadgesExist()
        {
            // Arrange
            var badges = new List<BadgeModel>
            {
                new BadgeModel("Badge 1", "Description 1", 1, 10, BadgeTypeCountEnum.NewbiesCount) { Id = 1 },
                new BadgeModel("Badge 2", "Description 2", 2, 20, BadgeTypeCountEnum.AssignedTasksCount) { Id = 2 }
            };

            using (var context = new CatchUpDbContext(_options))
            {
                context.Badges.AddRange(badges);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(_options))
            {
                var badgeService = new BadgeService(context, null);

                // Act
                var result = await badgeService.GetAll();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.Contains(result, b => b.Name == "Badge 1");
                Assert.Contains(result, b => b.Name == "Badge 2");
            }
        }
    }
}
