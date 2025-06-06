using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Models;
using catch_up_backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System;

namespace catch_up_backend.Tests
{
    public class FaqServiceTests
    {
        public static IEnumerable<object[]> GetFaqDtos()
        {
            yield return new object[] {
                new FaqDto { Question = "Q1", Answer = "A1", MaterialId = 1, CreatorId = Guid.NewGuid() }
            };
            yield return new object[] {
                new FaqDto { Question = "Q2", Answer = "A2", MaterialId = null, CreatorId = Guid.NewGuid() }
            };
        }

        [Theory]
        [MemberData(nameof(GetFaqDtos))]
        public async Task AddAsync_ShouldAddFaqToDatabase(FaqDto faqDto)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new CatchUpDbContext(options);
            var service = new FaqService(context);

            // Act
            var result = await service.AddAsync(faqDto);

            // Assert
            var faqInDb = await context.Faqs.FirstOrDefaultAsync(f => f.Id == result.Id);
            Assert.NotNull(faqInDb);
            Assert.Equal(faqDto.Question, faqInDb.Question);
            Assert.Equal(faqDto.Answer, faqInDb.Answer);
        }

        [Theory]
        [MemberData(nameof(GetFaqDtos))]
        public async Task EditAsync_ShoulEditExistingFaq(FaqDto newFaq)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var existingFaq = new FaqModel("Old Question", "Old Answer", 1, Guid.NewGuid());

            using (var context = new CatchUpDbContext(options))
            {
                context.Faqs.Add(existingFaq);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(options))
            {
                var service = new FaqService(context);

                // Act
                var result = await service.EditAsync(existingFaq.Id, newFaq);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(newFaq.Question, result.Question);
                Assert.Equal(newFaq.Answer, result.Answer);
                Assert.Equal(newFaq.MaterialId, result.MaterialId);
                Assert.Equal(newFaq.CreatorId, result.CreatorId);

                var faqInDb = await context.Faqs.FindAsync(existingFaq.Id);
                Assert.Equal(newFaq.Question, faqInDb.Question);
            }
        }

        [Fact]
        public async Task EditAsync_ShouldReturnNull_WhenFaqDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new CatchUpDbContext(options);
            var service = new FaqService(context);

            var nonExistingId = 999;

            var newFaqDto = new FaqDto
            {
                Question = "Updated Q",
                Answer = "Updated A",
                MaterialId = 1,
                CreatorId = Guid.NewGuid()
            };

            // Act
            var result = await service.EditAsync(nonExistingId, newFaqDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldMarkFaqAsDeleted_WhenExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var existingFaq = new FaqModel("Question", "Answer", 1, Guid.NewGuid());

            using (var context = new CatchUpDbContext(options))
            {
                context.Faqs.Add(existingFaq);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(options))
            {
                var service = new FaqService(context);

                // Act
                var result = await service.DeleteAsync(existingFaq.Id);

                // Assert
                Assert.True(result);

                var faqInDb = await context.Faqs.FindAsync(existingFaq.Id);
                Assert.Equal(StateEnum.Deleted, faqInDb.State);
            }
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenFaqNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new CatchUpDbContext(options);
            var service = new FaqService(context);

            // Act
            var result = await service.DeleteAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnFaqDto_WhenFaqExistsAndIsActive()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var existingFaq = new FaqModel("Question", "Answer", 1, Guid.NewGuid())
            {
                State = StateEnum.Active
            };

            using (var context = new CatchUpDbContext(options))
            {
                context.Faqs.Add(existingFaq);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(options))
            {
                var service = new FaqService(context);

                // Act
                var result = await service.GetByIdAsync(existingFaq.Id);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(existingFaq.Id, result.Id);
                Assert.Equal(existingFaq.Question, result.Question);
                Assert.Equal(existingFaq.Answer, result.Answer);
                Assert.Equal(existingFaq.MaterialId, result.MaterialId);
                Assert.Equal(existingFaq.CreatorId, result.CreatorId);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenFaqDoesNotExistOrIsNotActive()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var deletedFaq = new FaqModel("Question", "Answer", 1, Guid.NewGuid())
            {
                State = StateEnum.Deleted
            };

            using (var context = new CatchUpDbContext(options))
            {
                context.Faqs.Add(deletedFaq);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(options))
            {
                var service = new FaqService(context);

                // Act
                var result = await service.GetByIdAsync(deletedFaq.Id);
                var resultForNonExistent = await service.GetByIdAsync(999);

                // Assert
                Assert.Null(result);
                Assert.Null(resultForNonExistent);
            }
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnFaqs_WhenActiveFaqsExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var faq1 = new FaqModel("Q1", "A1", 1, Guid.NewGuid()) { State = StateEnum.Active };
            var faq2 = new FaqModel("Q2", "A2", 1, Guid.NewGuid()) { State = StateEnum.Deleted };
            var faq3 = new FaqModel("Q3", "A3", 2, Guid.NewGuid()) { State = StateEnum.Active };

            using (var context = new CatchUpDbContext(options))
            {
                context.Faqs.AddRange(faq1, faq2, faq3);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(options))
            {
                var service = new FaqService(context);

                // Act
                var (faqs, totalCount) = await service.GetAllAsync(page: 1, pageSize: 10);

                // Assert
                Assert.Equal(2, totalCount);
                Assert.Equal(2, faqs.Count);
                Assert.Contains(faqs, f => f.Question == "Q1");
                Assert.DoesNotContain(faqs, f => f.Question == "Q2");
                Assert.Contains(faqs, f => f.Question == "Q3");
            }
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoActiveFaqsExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatchUpDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new CatchUpDbContext(options))
            {
                var deletedFaq = new FaqModel("Deleted Q", "Deleted A", 1, Guid.NewGuid())
                {
                    State = StateEnum.Deleted
                };
                context.Faqs.Add(deletedFaq);
                await context.SaveChangesAsync();
            }

            using (var context = new CatchUpDbContext(options))
            {
                var service = new FaqService(context);

                // Act
                var (faqs, totalCount) = await service.GetAllAsync(page: 1, pageSize: 10);

                // Assert
                Assert.Empty(faqs);
                Assert.Equal(0, totalCount);
            }
        }

    }
}