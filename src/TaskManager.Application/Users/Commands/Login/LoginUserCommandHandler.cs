using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Abstractions.Inrefaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Application.Users.Commands.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResult>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService _tokenService;
    private readonly IPasswordHasher<User> _hasher;

    public LoginUserCommandHandler(
        IJwtTokenService tokenService,
        IMapper mapper,
        ApplicationDbContext context,
        IPasswordHasher<User> hasher)
    {
        _tokenService = tokenService;
        _mapper = mapper;
        _context = context;
        _hasher = hasher;
    }

    public async Task<LoginUserCommandResult> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email;
        var password = command.Password;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (user is null)
        {
            throw new Exception("Пользователь отсутствует!");
        }

        var passResult = _hasher.VerifyHashedPassword(null!, user.Password, password);

        if (passResult is not PasswordVerificationResult.Success)
        {
            throw new Exception("Неверный пароль");
        }

        var accessToken = _tokenService.GenerateAccessToken(user.Id);

        var result = new LoginUserCommandResult
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            AccessToken = accessToken
        };
        return result;
    }
}
