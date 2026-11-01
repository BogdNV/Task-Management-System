using FluentValidation;

namespace TaskManager.Application.Projects.Commands;

public class PutProjectCommandValidators : AbstractValidator<PutProjectCommand>
{
    public PutProjectCommandValidators()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название не может быть пустым")
            .MinimumLength(5).WithMessage("Минимум 5 символов")
            .MaximumLength(100).WithMessage("Максимум 100 символов");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Описание не может быть пустым")
            .MaximumLength(1000).WithMessage("Максимум 1000 символов");
    }
}
