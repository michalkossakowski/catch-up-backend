using catch_up_backend.Controllers;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using catch_up_backend.Repositories;
using catch_up_backend.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace catch_up_backend.Tests
{
    public class TaskServiceTests
    {
        private readonly DbContextOptions<CatchUpDbContext> _options;

        public TaskServiceTests()
        {
            _options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task AddAsync_ShouldAddTask()
        {
            // Arrange
            var newbieId = Guid.NewGuid();
            var mentorId = Guid.NewGuid();

            var newTask = new TaskDto
            {
                NewbieId = newbieId,
                AssigningId = mentorId,
                TaskContentId = 1,
                RoadMapPointId = 1,
                Deadline = DateTime.Now.AddDays(7),
                Priority = 4
            };
            var taskContent = new TaskContentModel(mentorId, 1, 1, "title", "description") 
            {
                Id = 1
            };

            var mockContentService = new Mock<ITaskContentService>();
            var mockUserService = new Mock<IUserService>();
            var mockNotificationService = new Mock<INotificationService>(); 
            var mockRoadMapPointService = new Mock<IRoadMapPointService>();
            var mockBadgeService = new Mock<IBadgeService>();

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new TaskService(context, 
                    mockContentService.Object,
                    new UserService(new UserRepository(context)),
                    mockNotificationService.Object,
                    mockRoadMapPointService.Object,
                    mockBadgeService.Object);


                context.Users.Add(new UserModel("Newbie", "Test", "newbie@test.com", "password", "Newbie","Junior") { Id = newbieId, AvatarId = 1 });
                context.Users.Add(new UserModel("Mentor", "Test", "mentor@test.com", "password", "Mentor", "Senior") { Id = mentorId, AvatarId = 1 });
                
                context.TaskContents.Add(taskContent);              
                context.SaveChanges();

                var users = await context.Users.ToListAsync();
                // Act
                var result = await service.AddAsync(newTask);

                // Assert
                var taskInDb = await context.Tasks.FirstOrDefaultAsync();
                Assert.NotNull(taskInDb);
                Assert.Equal(newTask.NewbieId, taskInDb.NewbieId);
                Assert.Equal(newTask.AssigningId, taskInDb.AssigningId);
                Assert.Equal(newTask.TaskContentId, taskInDb.TaskContentId);
            }
                
        }

        [Fact]
        public async Task EditAsync_ShouldEditTask()
        {
            // Arrange
            var task = new TaskModel
            {
                NewbieId = Guid.NewGuid(),
                AssigningId = Guid.NewGuid(),
                TaskContentId = 1,
                RoadMapPointId = 1,
                Deadline = DateTime.Now.AddDays(7),
                Priority = 4,
                State = StateEnum.Active
            };

            

            var updatedTask = new TaskDto
            {
                NewbieId = task.NewbieId,
                AssigningId = task.AssigningId,
                TaskContentId = task.TaskContentId,
                RoadMapPointId = task.RoadMapPointId,
                Deadline = DateTime.Now.AddDays(10),
                Priority = 4,
                Status = StatusEnum.InProgress
            };

            var mockContentService = new Mock<ITaskContentService>();
            var mockUserService = new Mock<IUserService>();
            var mockNotificationService = new Mock<INotificationService>();
            var mockRoadMapPointService = new Mock<IRoadMapPointService>();
            var mockBadgeService = new Mock<IBadgeService>();
            using (var context = new CatchUpDbContext(_options))
            {
                var service = new TaskService(context,
                    mockContentService.Object,
                    mockUserService.Object,
                    mockNotificationService.Object,
                    mockRoadMapPointService.Object,
                    mockBadgeService.Object);

                context.Tasks.Add(task);
                await context.SaveChangesAsync();

                // Act
                var result = await service.EditAsync(task.Id, updatedTask);

                // Assert
                Assert.True(result);
                var taskInDb = await context.Tasks.FindAsync(task.Id);
                Assert.Equal(updatedTask.Deadline, taskInDb.Deadline);
                Assert.Equal(updatedTask.Priority, taskInDb.Priority);
                Assert.Equal(updatedTask.Status, taskInDb.Status);
            }
            
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnTask()
        {
            // Arrange
            var task = new TaskModel
            {
                NewbieId = Guid.NewGuid(),
                AssigningId = Guid.NewGuid(),
                TaskContentId = 1,
                RoadMapPointId = 1,
                Deadline = DateTime.Now.AddDays(7),
                Priority = 4,
                State = StateEnum.Active
            };

            var mockContentService = new Mock<ITaskContentService>();
            var mockUserService = new Mock<IUserService>();
            var mockNotificationService = new Mock<INotificationService>();
            var mockRoadMapPointService = new Mock<IRoadMapPointService>();
            var mockBadgeService = new Mock<IBadgeService>();
            using (var context = new CatchUpDbContext(_options)) 
            {
                var service = new TaskService(context,
                    mockContentService.Object,
                    mockUserService.Object,
                    mockNotificationService.Object,
                    mockRoadMapPointService.Object,
                    mockBadgeService.Object);

                context.Tasks.Add(task);
                await context.SaveChangesAsync();

                // Act
                var result = await service.GetTaskByIdAsync(task.Id);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(task.Id, result.Id);
            }
            
        }

        [Fact]
        public async Task DeleteAsync_ShouldMarkTaskAsDeleted()
        {
            // Arrange
            var task = new TaskModel
            {
                NewbieId = Guid.NewGuid(),
                AssigningId = Guid.NewGuid(),
                TaskContentId = 1,
                RoadMapPointId = 1,
                Deadline = DateTime.Now.AddDays(7),
                Priority = 4,
                State = StateEnum.Active
            };

            var mockContentService = new Mock<ITaskContentService>();
            var mockUserService = new Mock<IUserService>();
            var mockNotificationService = new Mock<INotificationService>();
            var mockRoadMapPointService = new Mock<IRoadMapPointService>();
            var mockBadgeService = new Mock<IBadgeService>();
            using (var context = new CatchUpDbContext(_options)) 
            {
                var service = new TaskService(context,
                    mockContentService.Object,
                    mockUserService.Object,
                    mockNotificationService.Object,
                    mockRoadMapPointService.Object,
                    mockBadgeService.Object);

                context.Tasks.Add(task);
                await context.SaveChangesAsync();
                // Act
                var result = await service.DeleteAsync(task.Id);

                // Assert
                Assert.True(result);
                var taskInDb = await context.Tasks.FindAsync(task.Id);
                Assert.Equal(StateEnum.Deleted, taskInDb.State);
            }

            
        }
    }
}
