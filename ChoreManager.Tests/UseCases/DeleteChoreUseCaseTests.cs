using ChoreManager.Application.UseCases.ChoreUseCases.Implementations;
using ChoreManager.Domain.Interfaces;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace ChoreManager.Tests.UseCases
{
    public class DeleteChoreUseCaseTests
    {
        private readonly Mock<IChoreRepository> _repositoryMock;

        public DeleteChoreUseCaseTests()
        {
            _repositoryMock = new Mock<IChoreRepository>();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldCallDeleteAsync_WhenIdIsValid()
        {
            var validId = Guid.NewGuid();
            DeleteChoreUseCase useCase = new (_repositoryMock.Object);

            await useCase.ExecuteAsync(validId);

            _repositoryMock.Verify(repo => repo.DeleteAsync(validId), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            var invalidId = Guid.Empty;
            DeleteChoreUseCase useCase = new (_repositoryMock.Object);

            Func<Task> act = async () => await useCase.ExecuteAsync(invalidId);

            await act.Should().ThrowAsync<ArgumentException>().WithMessage("El ID de la tarea no puede ser Guid.Empty.");
            _repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}
