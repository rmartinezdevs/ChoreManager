using ChoreManager.Application.DTOs;
using ChoreManager.Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace ChoreManager.Tests.Validators
{
    public class ChoreValidatorTests
    {
        private readonly ChoreValidator _validator;

        public ChoreValidatorTests()
        {
            _validator = new ChoreValidator();
        }

        [Fact]
        public void Should_HaveError_When_TitleIsEmpty()
        {
            var chore = new ChoreDto
            {
                Title = string.Empty,
                Description = "Descripción",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(chore);

            result.ShouldHaveValidationErrorFor(c => c.Title)
                .WithErrorMessage("Titulo de la tarea obligatorio");
        }

        [Fact]
        public void Should_HaveError_When_DescriptionIsEmpty()
        {
            var chore = new ChoreDto
            {
                Title = "Título",
                Description = string.Empty,
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(chore);

            result.ShouldHaveValidationErrorFor(c => c.Description)
                .WithErrorMessage("Descripción de la tarea obligatoria");
        }

        [Fact]
        public void Should_HaveError_When_DueDateIsInPast()
        {
            var chore = new ChoreDto
            {
                Title = "Título",
                Description = "Descripción",
                DueDate = DateTime.Now.AddDays(-1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(chore);

            result.ShouldHaveValidationErrorFor(c => c.DueDate)
                .WithErrorMessage("La fecha de vencimiento no puede ser en el pasado");
        }

        [Fact]
        public void Should_HaveError_When_StatusIdIsInvalid()
        {
            var chore = new ChoreDto
            {
                Title = "Título",
                Description = "Descripción",
                DueDate = DateTime.Now.AddDays(1),
                Status = (Domain.Enums.ChoreEnums.ChoreStatus)999,
                AssignedUserId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(chore);

            result.ShouldHaveValidationErrorFor(c => c.Status)
                .WithErrorMessage("El estado no es válido.");
        }

        [Fact]
        public void Should_HaveError_When_AssignedUserIdIsInvalid()
        {
            var chore = new ChoreDto
            {
                Title = "Título",
                Description = "Descripción",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.Empty
            };

            var result = _validator.TestValidate(chore);

            result.ShouldHaveValidationErrorFor(c => c.AssignedUserId)
                .WithErrorMessage("Debe asignarse un usuario válido.");
        }

        [Fact]
        public void Should_PassValidation_WithValidData()
        {
            var chore = new ChoreDto
            {
                Title = "Valid Title",
                Description = "Valid Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = Domain.Enums.ChoreEnums.ChoreStatus.Pending,
                AssignedUserId = Guid.NewGuid()
            };

            var result = _validator.TestValidate(chore);

            result.IsValid.Should().BeTrue();
        }
    }
}
