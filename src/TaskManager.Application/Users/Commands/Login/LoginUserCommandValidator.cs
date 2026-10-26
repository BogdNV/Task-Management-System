using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace TaskManager.Application.Users.Commands.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(l => l.Email)
            .NotEmpty()
            .EmailAddress().WithMessage("Некорректный email")
            .MaximumLength(5)
            .MinimumLength(100);

        RuleFor(l => l.Password)
            .NotEmpty();

    }
}
