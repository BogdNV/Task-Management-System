using FluentValidation;

namespace TaskManager.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(t => t.ProjectId)
            .NotEmpty();

        RuleFor(t => t.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(t => t.Description)
            .MaximumLength(1000);
    }
}
