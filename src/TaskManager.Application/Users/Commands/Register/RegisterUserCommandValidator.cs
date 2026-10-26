using FluentValidation;

namespace TaskManager.Application.Users.Commands;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty().WithMessage("Имя обязательно")
            .MinimumLength(3)
            .MaximumLength(25);

        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress().WithMessage("Некорректный email")
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(u => u.Password)
            .NotEmpty();
    }
}
