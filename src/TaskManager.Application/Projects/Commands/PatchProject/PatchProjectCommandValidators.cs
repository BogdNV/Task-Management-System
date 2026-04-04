using FluentValidation;

namespace TaskManager.Application.Projects.Commands.PatchProject;

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
