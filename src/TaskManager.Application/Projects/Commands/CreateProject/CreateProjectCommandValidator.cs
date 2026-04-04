using FluentValidation;

namespace TaskManager.Application.Projects.Commands.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название проекта обязательно")
            .MinimumLength(5).WithMessage("Минимум 5 символов")
            .MaximumLength(100).WithMessage("Максимум 100 символов");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Описание обязательно")
            .MaximumLength(1000).WithMessage("Максимум 1000 символов");

        RuleFor(x => x.OwnerId)
            .GreaterThan(0).WithMessage("Неверный ID владельца");
    }
}
