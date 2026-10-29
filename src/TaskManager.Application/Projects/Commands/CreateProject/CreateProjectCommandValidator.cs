using FluentValidation;

namespace TaskManager.Application.Projects.Commands;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название проекта обязательно")
            .MinimumLength(3).WithMessage("Минимум 3 символа")
            .MaximumLength(100).WithMessage("Максимум 100 символов");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Описание обязательно")
            .MaximumLength(1000).WithMessage("Максимум 1000 символов");
    }
}
