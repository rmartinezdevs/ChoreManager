using AutoMapper;
using ChoreManager.Application.UseCases.ChoreUseCases.Implementations;
using ChoreManager.Domain.Entities;
using ChoreManager.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ChoreManager.Tests.UseCases
{
    public class SelectChoreByIdUseCaseTests
    {
        private readonly Mock<IChoreRepository> _repositoryMock;
        private readonly IMapper _mapper;

        public SelectChoreByIdUseCaseTests()
        {
            _repositoryMock = new Mock<IChoreRepository>();
            _mapper = MapperTestHelper.CreateMapper();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnAChore()
        {
            //Arrange
            SelectChoreByIdUseCase useCase = new (_repositoryMock.Object, _mapper);

            var validId = Guid.NewGuid();
            var chore = new Chore
            {
                Id = validId,
                Title = "Test Chore 1",
                Description = "Description 1",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(validId))
                .ReturnsAsync(chore);

            //Act
            var result = await useCase.ExecuteAsync(validId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(chore, options => options.ComparingByMembers<Chore>());

            _repositoryMock.Verify(repo => repo.GetByIdAsync(validId), Times.Once());
        }

        [Fact]
        public async Task ExecuteAsyncShouldThrowArgumentExceptionWhenIdIsEmpty()
        {
            //Arrange
            var invalidId = Guid.Empty;
            SelectChoreByIdUseCase useCase = new (_repositoryMock.Object, _mapper);

            //Act
            Func<Task> act = async () => await useCase.ExecuteAsync(invalidId);

            //Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("El ID de la tarea no puede ser Guid.Empty.");

            _repositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Never());
        }

        [Fact]
        public async Task ExecuteAsyncShouldReturnNullWhenChoreDoesNotExist()
        {
            //Arrange
            var nonExistentId = Guid.NewGuid();
            SelectChoreByIdUseCase useCase = new (_repositoryMock.Object, _mapper);

            _repositoryMock.Setup(repo => repo.GetByIdAsync(nonExistentId))
                .ReturnsAsync(default(Chore));

            //Act
            var result = await useCase.ExecuteAsync(nonExistentId);

            //Assert
            result.Should().BeNull();

            _repositoryMock.Verify(repo => repo.GetByIdAsync(nonExistentId), Times.Once());
        }
    }
}
