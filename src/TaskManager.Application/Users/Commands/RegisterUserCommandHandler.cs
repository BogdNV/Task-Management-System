using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Abstractions.Inrefaces;
using TaskManager.Application.Mappers;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Entities;
using TaskManager.Infrastructure.Mappers;

namespace TaskManager.Application.Users.Commands;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserCommandResult>
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenService _tokenService;

    public RegisterUserCommandHandler(
        ApplicationDbContext context,
        IPasswordHasher<User> passwordHasher,
        IJwtTokenService token)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = token;
    }

    public async Task<RegisterUserCommandResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        string email = request.Email;

        bool userExists = await _context.Users
            .AnyAsync(u => u.Email == email, cancellationToken);

        if (userExists)
        {
            // throw exception
        }

        request.Password = _passwordHasher.HashPassword(null, request.Password);

        var user = request.ToDomain().ToEntity();

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Name);

        return new RegisterUserCommandResult()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            AccessToken = accessToken
        };

    }
}
