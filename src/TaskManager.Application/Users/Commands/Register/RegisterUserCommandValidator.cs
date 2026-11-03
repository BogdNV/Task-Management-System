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
            .EmailAddress().WithMessage("Некорректный email");

        RuleFor(u => u.Password)
            .MinimumLength(5)
            .NotEmpty();
    }
}
