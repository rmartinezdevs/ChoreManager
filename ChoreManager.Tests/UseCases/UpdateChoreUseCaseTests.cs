using AutoMapper;
using ChoreManager.Application.DTOs;
using ChoreManager.Application.UseCases.ChoreUseCases.Implementations;
using ChoreManager.Domain.Entities;
using ChoreManager.Domain.Interfaces;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace ChoreManager.Tests.UseCases
{
    public class UpdateChoreUseCaseTests
    {
        private readonly Mock<IChoreRepository> _repositoryMock;
        private readonly Mock<IValidator<ChoreDto>> _validatorMock;
        private readonly IMapper _mapper;

        public UpdateChoreUseCaseTests(Mock<IChoreRepository> repositoryMock, Mock<IValidator<ChoreDto>> validatorMock, IMapper mapper)
        {
            _repositoryMock = new Mock<IChoreRepository>();
            _validatorMock = new Mock<IValidator<ChoreDto>>();
            _mapper = MapperTestHelper.CreateMapper();
        }

        [Fact]
        public async Task ExecuteAsyncShouldUpdateChoreWhenDataIsValid()
        {
            //Arrange
            UpdateChoreUseCase useCase = new (_repositoryMock.Object, _validatorMock.Object, _mapper);

            var validId = Guid.NewGuid();

            var existingChore = new Chore
            {
                Id = validId,
                Title = "Old Title",
                Description = "Old Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            var updatedChoreDto = new ChoreDto
            {
                Id = validId,
                Title = "Updated Title",
                Description = "Updated Description",
                DueDate = DateTime.Now.AddDays(2),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Completed,
                AssignedUserId = Guid.NewGuid()
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(validId))
                 .ReturnsAsync(existingChore);

            _validatorMock.Setup(v => v.ValidateAsync(It.Is<ChoreDto>(dto =>
                    dto.Id == updatedChoreDto.Id &&
                    dto.Title == updatedChoreDto.Title), default))
                .ReturnsAsync(new ValidationResult());

            //Act
            await useCase.ExecuteAsync(updatedChoreDto);

            //Asserts
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Chore>(c =>
                c.Id == validId &&
                c.Title == updatedChoreDto.Title &&
                c.Description == updatedChoreDto.Description &&
                c.DueDate == updatedChoreDto.DueDate &&
                c.Status == updatedChoreDto.Status &&
                c.AssignedUserId == updatedChoreDto.AssignedUserId
            )), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsyncShouldThrowArgumentExceptionWhenIdIsEmpty()
        {
            //Arrange
            UpdateChoreUseCase useCase = new (_repositoryMock.Object, _validatorMock.Object, _mapper);
            var invalidChoreDto = new ChoreDto
            {
                Id = Guid.Empty,
                Title = "Title",
                Description = "Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            //Act
            Func<Task> act = async () => await useCase.ExecuteAsync(invalidChoreDto);

            //Asserts
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("El ID de la tarea no puede ser Guid.Empty.");

            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Chore>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsyncShouldThrowInvalidOperationExceptionWhenChoreDoesNotExist()
        {
            //Arrange
            var nonExistentId = Guid.NewGuid();
            var choreDto = new ChoreDto
            {
                Id = nonExistentId,
                Title = "Title",
                Description = "Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(nonExistentId))
                .ReturnsAsync((Chore)null);

            var useCase = new UpdateChoreUseCase(_repositoryMock.Object, _validatorMock.Object, _mapper);


            //Act
            Func<Task> act = async () => await useCase.ExecuteAsync(choreDto);

            //Asserts
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"No se encontró ninguna tarea con el ID {nonExistentId}.");
        }

        [Fact]
        public async Task ExecuteAsyncShouldNotCallRepositoryWhenValidationFails()
        {
            //Arrange
            UpdateChoreUseCase useCase = new (_repositoryMock.Object, _validatorMock.Object, _mapper);
            var choreDto = new ChoreDto
            {
                Id = Guid.NewGuid(),
                Title = "Invalid Chore",
                Description = "Invalid Chore",
                DueDate = DateTime.Now.AddDays(-1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            var validationFailures = new[]
            {
                new ValidationFailure("DueDate", "La fecha de vencimiento no puede ser en el pasado")
            };

            _validatorMock.Setup(validator => validator.ValidateAsync(choreDto, default))
                .ReturnsAsync(new ValidationResult(validationFailures));

            //Act
            Func<Task> act = async() => await useCase.ExecuteAsync(choreDto);

            //Asserts
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Validation Exception");

            _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Chore>()), Times.Never);
        }
    }
}
