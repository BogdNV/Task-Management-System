using FluentValidation;

namespace TaskManager.Application.Users.Commands.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(l => l.Email)
            .NotEmpty()
            .EmailAddress().WithMessage("Некорректный email");

        RuleFor(l => l.Password)
            .MinimumLength(5)
            .NotEmpty();

    }
}
