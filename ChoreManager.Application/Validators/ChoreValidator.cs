using ChoreManager.Application.DTOs;
using FluentValidation;

namespace ChoreManager.Application.Validators
{
    public class ChoreValidator : AbstractValidator<ChoreDto>
    {
        public ChoreValidator()
        {
            RuleFor(chore => chore.Title)
                .NotEmpty().WithMessage("Titulo de la tarea obligatorio")
                .MaximumLength(100).WithMessage("Maximo 100 caracteres para el titulo");

            RuleFor(chore => chore.Description)
                .NotEmpty().WithMessage("Descripción de la tarea obligatoria");

            RuleFor(chore => chore.DueDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow.ToLocalTime()).WithMessage("La fecha de vencimiento no puede ser en el pasado");

            RuleFor(chore => chore.Status)
                .IsInEnum().WithMessage("El estado no es válido.");

            RuleFor(chore => chore.AssignedUserId)
                .NotEmpty().WithMessage("Debe asignarse un usuario válido.");
        }
    }
}
