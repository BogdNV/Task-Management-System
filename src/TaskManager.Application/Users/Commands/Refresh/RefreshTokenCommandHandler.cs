using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Abstractions.Inrefaces;
using TaskManager.Application.Users.DTO;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Application.Users.Commands.Refresh;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenCommandResult>
{
    private readonly IJwtTokenService _tokenService;
    private readonly ApplicationDbContext _context;

    public RefreshTokenCommandHandler(ApplicationDbContext context, IJwtTokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<RefreshTokenCommandResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == request.RefreshToken, cancellationToken);
        if (token is null || !token.IsActive)
            throw new UnauthorizedAccessException("Недействительно или истек срок действия токена");

        var accessToken = _tokenService.GenerateAccessToken(token.UserId);
        var refreshToken = _tokenService.GenerateRefreshToken();

        await _tokenService.RevokeRefreshToken(token.Token, cancellationToken, refreshToken);
        await _tokenService.SaveRefreshToken(token.UserId, refreshToken, 7, cancellationToken);

        return new RefreshTokenCommandResult
        {
            Token = new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken }
        };
    }
}
