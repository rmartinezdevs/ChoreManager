using AutoMapper;
using ChoreManager.Domain.Entities;
using ChoreManager.Infrastructure.Context;
using ChoreManager.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Xunit;

namespace ChoreManager.Tests.Repositories
{
    public class ChoreRepositoryTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddChoreToDatabase()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var mapper = MapperTestHelper.CreateMapper();
            ChoreRepository repository = new (dbContext, mapper);
            Chore chore = new Chore
            {
                Id = Guid.NewGuid(),
                Title = "Test Chore",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            // Act
            await repository.AddAsync(chore);
            var result = await dbContext.Chores.FirstOrDefaultAsync(c => c.Id == chore.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Title.Should().Be("Test Chore");
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenChoreIdAlreadyExist()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var mapper = MapperTestHelper.CreateMapper();
            ChoreRepository repository = new (dbContext, mapper);
            var existentId = Guid.NewGuid();

            Chore chore1 = new Chore
            {
                Id = existentId,
                Title = "Test Chore 1",
                Description = "Test Description 1",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            Chore chore2 = new Chore
            {
                Id = existentId,
                Title = "Test Chore 2",
                Description = "Test Description 2",
                DueDate = DateTime.Now.AddDays(2),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.InProgress,
                AssignedUserId = Guid.NewGuid()

            };

            await repository.AddAsync(chore1);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await repository.AddAsync(chore2)
            );

            exception.Message.Should().Contain($"A chore with ID {existentId} already exists.");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateChoreFromDatabase()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var mapper = MapperTestHelper.CreateMapper();
            ChoreRepository repository = new (dbContext, mapper);
            var existentId = Guid.NewGuid();

            Chore chore1 = new Chore
            {
                Id = existentId,
                Title = "Test Chore",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            Chore chore2 = new Chore
            {
                Id = existentId,
                Title = "Test Chore - Updateado",
                Description = "Test Description 2",
                DueDate = DateTime.Now.AddDays(3),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            // Act
            await dbContext.Chores.AddAsync(chore1);
            await dbContext.SaveChangesAsync();
            var addedChore = await dbContext.Chores.FirstOrDefaultAsync(c => c.Id == existentId);
            addedChore.Should().NotBeNull();

            await repository.UpdateAsync(chore2);

            var result = await dbContext.Chores.FirstOrDefaultAsync(c => c.Id == existentId);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Test Chore - Updateado");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenChoreIdDoesNotExist()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var mapper = MapperTestHelper.CreateMapper();
            var repository = new ChoreRepository(dbContext, mapper);
            var nonExistentId = Guid.NewGuid();

            Chore chore = new Chore
            {
                Id = nonExistentId,
                Title = "Test Chore",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await repository.UpdateAsync(chore)
            );

            exception.Message.Should().Contain($"No se encontró ninguna tarea con el ID {nonExistentId}");
        }
        
        [Fact]
        public async Task DeleteAsync_ShouldDeleteChoreFromDatabase()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var mapper = MapperTestHelper.CreateMapper();
            ChoreRepository repository = new (dbContext, mapper);
            Chore chore = new Chore
            {
                Id = Guid.NewGuid(),
                Title = "Test Chore",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            // Act
            await dbContext.Chores.AddAsync(chore);
            await dbContext.SaveChangesAsync();

            var addedChore = await dbContext.Chores.FirstOrDefaultAsync(c => c.Id == chore.Id);
            addedChore.Should().NotBeNull();

            await repository.DeleteAsync(chore.Id);

            var result = await dbContext.Chores.FirstOrDefaultAsync(c => c.Id == chore.Id);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenChoreDoesNotExist()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var mapper = MapperTestHelper.CreateMapper();
            var repository = new ChoreRepository(dbContext, mapper);
            var nonExistentId = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await repository.DeleteAsync(nonExistentId)
            );

            exception.Message.Should().Contain($"No se encontró ninguna tarea con el ID {nonExistentId}");
        }

        [Fact]
        public async Task GetAllAsync_ShouldGetAllChoresFromDatabase()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var mapper = MapperTestHelper.CreateMapper();
            ChoreRepository repository = new (dbContext, mapper);

            var chores = new[]
            {
                new Chore
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Chore 1",
                    Description = "Test Description 1",
                    DueDate = DateTime.Now.AddDays(1),
                    Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                    AssignedUserId = Guid.NewGuid()
                },
                new Chore
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Chore 2",
                    Description = "Test Description 2",
                    DueDate = DateTime.Now.AddDays(2),
                    Status = Domain.Enums.ChoreEnums.ChoreStatus.InProgress,
                    AssignedUserId = Guid.NewGuid()
                }
            };

            await dbContext.Chores.AddRangeAsync(chores);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Count().Should().Be(chores.Length);

            foreach (var chore in chores)
            {
                result.Should().ContainSingle(c =>
                    c.Id == chore.Id &&
                    c.Title == chore.Title &&
                    c.Description == chore.Description &&
                    c.DueDate.Date == chore.DueDate.Date &&
                    c.Status == chore.Status &&
                    c.AssignedUserId == chore.AssignedUserId);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldGetChoreByIdFromDatabase()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var mapper = MapperTestHelper.CreateMapper();
            ChoreRepository repository = new(dbContext, mapper);
            var existentId = Guid.NewGuid();

            Chore chore = new Chore
            {
                Id = existentId,
                Title = "Test Chore",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            // Act
            await dbContext.Chores.AddAsync(chore);
            await dbContext.SaveChangesAsync();

            var result = await repository.GetByIdAsync(existentId);
            
            // Assert
            result.Should().NotBeNull();
            result!.Title.Should().Be("Test Chore");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowException_WhenChoreIdDoesNotExist()
        {
            // Arrange
            var dbContext = CreateInMemoryDbContext();
            var mapper = MapperTestHelper.CreateMapper();
            ChoreRepository repository = new (dbContext, mapper);
            var nonExistentId = Guid.NewGuid();

            Chore chore = new Chore
            {
                Id = nonExistentId,
                Title = "Test Chore",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await repository.GetByIdAsync(nonExistentId)
            );

            exception.Message.Should().Contain($"No se encontró ninguna tarea con el ID {nonExistentId}");
        }
    }
}
