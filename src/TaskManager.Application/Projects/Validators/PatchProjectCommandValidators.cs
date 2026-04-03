using FluentValidation;
using TaskManager.Application.Projects.Commands.UpdateProject;

namespace TaskManager.Application.Projects.Validators;

public class PatchProjectCommandValidators : AbstractValidator<PatchProjectCommand>
{
    public PatchProjectCommandValidators()
    {
        RuleFor(x => x.Name)
            .MinimumLength(5).WithMessage("Минимум 5 символов")
            .MaximumLength(100).WithMessage("Максимум 100 символов")
            .When(x => !string.IsNullOrEmpty(x.Name), ApplyConditionTo.CurrentValidator);

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Максимум 1000 символов")
            .When(x => !string.IsNullOrEmpty(x.Description), ApplyConditionTo.CurrentValidator);
    }
}
