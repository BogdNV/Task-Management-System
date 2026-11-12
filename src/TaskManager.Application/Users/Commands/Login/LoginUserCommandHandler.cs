using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Abstractions.Inrefaces;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Users.DTO;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Application.Users.Commands.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResult>
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtTokenService _tokenService;
    private readonly IPasswordHasher<User> _hasher;

    public LoginUserCommandHandler(
        IJwtTokenService tokenService,
        ApplicationDbContext context,
        IPasswordHasher<User> hasher)
    {
        _tokenService = tokenService;
        _context = context;
        _hasher = hasher;
    }

    public async Task<LoginUserCommandResult> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email;
        var password = command.Password;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (user is null)
            throw new NotFoundException("Пользователь отсутствует!");

        var passResult = _hasher.VerifyHashedPassword(null!, user.Password, password);

        if (passResult is not PasswordVerificationResult.Success)
            throw new UnauthorizedAccessException("Неверный пароль");

        var accessToken = _tokenService.GenerateAccessToken(user.Id);

        var refreshToken = await _tokenService.SaveRefreshToken(user.Id, cancellationToken);

        var result = new LoginUserCommandResult
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Token = new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken }
        };
        return result;
    }
}
