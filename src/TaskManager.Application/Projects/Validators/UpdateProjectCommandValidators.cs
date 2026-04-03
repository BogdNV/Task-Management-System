using FluentValidation;
using TaskManager.Application.Projects.Commands.UpdateProject;

namespace TaskManager.Application.Projects.Validators;

public class UpdateProjectCommandValidators : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidators()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название не может быть пустым")
            .MinimumLength(5).WithMessage("Минимум 5 символов")
            .MaximumLength(100).WithMessage("Максимум 100 символов");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Описание не может быть пустым")
            .MaximumLength(1000).WithMessage("Максимум 1000 символов");
    }
}
