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
    public class CreateChoreUseCaseTests
    {
        private readonly Mock<IChoreRepository> _repositoryMock;
        private readonly Mock<IValidator<CreateChoreDto>> _validatorMock;
        private readonly IMapper _mapper;

        public CreateChoreUseCaseTests()
        {
            _repositoryMock = new Mock<IChoreRepository>();
            _validatorMock = new Mock<IValidator<CreateChoreDto>>();
            _mapper = MapperTestHelper.CreateMapper();
        }

        [Fact]
        public async Task ExecuteAsyncShouldCallRepositoryWithMappedChore()
        {
            //Arrange
            CreateChoreUseCase useCase = new(_repositoryMock.Object, _validatorMock.Object, _mapper);

            var createChoreDto = new CreateChoreDto
            {
                Title = "Valid Title",
                Description = "Valid Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            _validatorMock.Setup(v => v.ValidateAsync(createChoreDto, default))
                .ReturnsAsync(new ValidationResult());

            //Act
            await useCase.ExecuteAsync(createChoreDto);

            //Assert
            _repositoryMock.Verify(repo => repo.AddAsync(It.Is<Chore>(chore =>
                chore.Title == createChoreDto.Title &&
                chore.Description == createChoreDto.Description &&
                chore.DueDate == createChoreDto.DueDate &&
                chore.Status == createChoreDto.Status &&
                chore.AssignedUserId == createChoreDto.AssignedUserId
            )),Times.Once());
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowExceptionWhenDtoIsInvalid()
        {
            //Arrange
            CreateChoreUseCase useCase = new (_repositoryMock.Object, _validatorMock.Object, _mapper);

            var createChoreDto = new CreateChoreDto
            {
                Title = string.Empty,
                Description = "Valid Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            var validationFailures = new[] { new ValidationFailure("Title", "Titulo de la tarea obligatorio") };

            _validatorMock.Setup(v => v.ValidateAsync(createChoreDto,default))
                .ReturnsAsync(new ValidationResult(validationFailures));

            //Act
            Func<Task> act = async () => await useCase.ExecuteAsync(createChoreDto);

            //Assert
            var exception = await act.Should().ThrowAsync<FluentValidation.ValidationException>();
            exception.Which.Errors.Should().ContainSingle(e =>
                e.PropertyName == "Title" && e.ErrorMessage == "Titulo de la tarea obligatorio");
        }

        [Fact]
        public async Task ExecuteAsync_ShouldNotCallRepositoryWhenValidationFails()
        {
            //Arrange
            CreateChoreUseCase useCase = new (_repositoryMock.Object, _validatorMock.Object, _mapper);

            var createChoreDto = new CreateChoreDto
            {
                Title = string.Empty,
                Description = "Valid Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            var validationFailures = new[] { new ValidationFailure("Title", "Titulo de la tarea obligatorio") };

            _validatorMock.Setup(v => v.ValidateAsync(createChoreDto, default))
                .ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            Func<Task> act = async () => await useCase.ExecuteAsync(createChoreDto);

            //Assert
            var exception = await act.Should().ThrowAsync<FluentValidation.ValidationException>();
            exception.Which.Errors.Should().ContainSingle(e =>
                e.PropertyName == "Title" && e.ErrorMessage == "Titulo de la tarea obligatorio");

            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Chore>()), Times.Never());
        }
    }
}
