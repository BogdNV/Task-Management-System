using FluentValidation;

namespace TaskManager.Application.Projects.Commands;

public class PatchProjectCommandValidators : AbstractValidator<PatchProjectCommand>
{
    public PatchProjectCommandValidators()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x)
            .Must(x => x.Title is not null && x.Description is not null)
            .WithMessage("Необходимо указать хотя бы одно из полей: Title, Description.");

        When(x => x.Title is not null, () =>
        {
            RuleFor(x => x.Title).MaximumLength(100);
        });

        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description).MaximumLength(1000);
        });
    }
}
