using catch_up_backend.Controllers;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces.RepositoryInterfaces;
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
using Xunit.Sdk;
namespace catch_up_backend.Tests
{
    public class UserServiceTests
    {
        private readonly DbContextOptions<CatchUpDbContext> _options;
        public UserServiceTests()
        {
            _options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }
        [Fact]
        public async Task AddAsync_ShouldAddUser()
        {
            // Arrange
            var newbieId = Guid.NewGuid();


            using (var context = new CatchUpDbContext(_options))
            {
                var service = new UserService(new UserRepository(context));
                // Act
                var result = await service.Add(new UserDto {
                    Id = newbieId,
                    Name = "Newbie",
                    Surname = "Test",
                    Email = "newbie@test.com",
                    Password = "password",
                    Type = "Newbie",
                    Position = "Junior Developer",
                    AvatarId = 1,
                    Counters = new Dictionary<BadgeTypeCountEnum, int>()

                });

                // Assert
                var newUser = await context.Users.FindAsync(result.Id);
                Assert.NotNull(newUser);

            }
        }

        [Fact]
        public async Task GetById_ShouldReturnUser()
        {
            // Arrange
            var newbieId = Guid.NewGuid();

            using (var context = new CatchUpDbContext(_options))
            {
                var service = new UserService(new UserRepository(context));
                
                context.Users.Add(new UserModel("user", "surname", "test@test.com", "passwd", "Newbie", "Junior") { Id = newbieId });
                await context.SaveChangesAsync();
                // Act
                var result = await service.GetById(newbieId);
                // Assert
                Assert.NotNull(result);
                Assert.Equal("user", result.Name);

            }
        }


    }
}
