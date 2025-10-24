using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catch_up_backend.Database;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using catch_up_backend.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace catch_up_backend.Tests
{
    public class NewbieMentorServiceTests
    {
        private readonly DbContextOptions<CatchUpDbContext> _options;

        public NewbieMentorServiceTests()
        {
            _options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task AssignNewbieToMentor_ShouldAssign_WhenValidIdsProvided()
        {
            // Arrange
            var newbieId = Guid.NewGuid();
            var mentorId = Guid.NewGuid();

            var newbie = new UserModel("Newbie", "Test", "newbie@test.com", "password", "Newbie") { Id = newbieId, State = StateEnum.Active };
            var mentor = new UserModel("Mentor", "Test", "mentor@test.com", "password", "Mentor") { Id = mentorId, State = StateEnum.Active };

            var mockNotificationService = new Mock<INotificationService>();
            var mockBadgeService = new Mock<IBadgeService>();

            using (var context = new CatchUpDbContext(_options))
            {
                context.Users.AddRange(newbie, mentor);
                await context.SaveChangesAsync();

                var service = new NewbieMentorService(context, mockNotificationService.Object, mockBadgeService.Object);

                // Act
                var result = await service.AssignNewbieToMentor(newbieId, mentorId);

                // Assert
                Assert.True(result);
                var assignment = context.NewbiesMentors.FirstOrDefault();
                Assert.NotNull(assignment);
                Assert.Equal(newbieId, assignment.NewbieId);
                Assert.Equal(mentorId, assignment.MentorId);
                Assert.Equal(StateEnum.Active, assignment.State);
            }
        }

        [Fact]
        public async Task AssignNewbieToMentor_ShouldReturnFalse_WhenInvalidIdsProvided()
        {
            // Arrange
            var newbieId = Guid.NewGuid();
            var mentorId = Guid.NewGuid();

            using (var context = new CatchUpDbContext(_options))
            {
                var mockNotificationService = new Mock<INotificationService>();
                var mockBadgeService = new Mock<IBadgeService>();

                var service = new NewbieMentorService(context, mockNotificationService.Object, mockBadgeService.Object);

                // Act
                var result = await service.AssignNewbieToMentor(newbieId, mentorId);

                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task SetAssignmentState_ShouldUpdateState_WhenAssignmentExists()
        {
            // Arrange
            var newbieId = Guid.NewGuid();
            var mentorId = Guid.NewGuid();

            var newbie = new UserModel("Newbie", "Test", "newbie@test.com", "password", "Newbie") { Id = newbieId, State = StateEnum.Active };
            var mentor = new UserModel("Mentor", "Test", "mentor@test.com", "password", "Mentor") { Id = mentorId, State = StateEnum.Active };

            var assignment = new NewbieMentorModel(newbieId, mentorId)
            {
                State = StateEnum.Active,
                StartDate = DateTime.Now
            };

            using (var context = new CatchUpDbContext(_options))
            {
                context.Users.AddRange(mentor, newbie);
                context.NewbiesMentors.Add(assignment);
                await context.SaveChangesAsync();
            }

            var mockNotificationService = new Mock<INotificationService>();
            var mockBadgeService = new Mock<IBadgeService>();

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new NewbieMentorService(context, mockNotificationService.Object, mockBadgeService.Object);

                // Act
                var result = await service.SetAssignmentState(newbieId, mentorId, StateEnum.Deleted);

                // Assert
                Assert.True(result);
                var updatedAssignment = context.NewbiesMentors.First();
                Assert.Equal(StateEnum.Deleted, updatedAssignment.State);
                Assert.NotNull(updatedAssignment.EndDate);
            }
        }

        [Fact]
        public async Task GetAssignments_ShouldReturnAssignments_WhenValidIdProvided()
        {
            // Arrange
            var mentorId = Guid.NewGuid();
            var newbieId = Guid.NewGuid();

            var mentor = new UserModel("Mentor", "Test", "mentor@test.com", "password", "Mentor") { Id = mentorId, State = StateEnum.Active };
            var newbie = new UserModel("Newbie", "Test", "newbie@test.com", "password", "Newbie") { Id = newbieId, State = StateEnum.Active };

            var assignment = new NewbieMentorModel(newbieId, mentorId)
            {
                State = StateEnum.Active,
                StartDate = DateTime.Now
            };

            using (var context = new CatchUpDbContext(_options))
            {
                context.Users.AddRange(mentor, newbie);
                context.NewbiesMentors.Add(assignment);
                await context.SaveChangesAsync();
            }

            var mockNotificationService = new Mock<INotificationService>();
            var mockBadgeService = new Mock<IBadgeService>();

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new NewbieMentorService(context, mockNotificationService.Object, mockBadgeService.Object);

                // Act
                var result = await service.GetAssignments(mentorId, RoleEnum.Mentor);

                // Assert
                Assert.Single(result);
                Assert.Equal(newbieId, result.First().Id);
            }
        }
    }
}
