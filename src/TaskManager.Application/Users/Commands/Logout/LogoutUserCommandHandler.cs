using MediatR;
using TaskManager.Abstractions.Inrefaces;

namespace TaskManager.Application.Users.Commands.Logout;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand>
{
    private readonly IJwtTokenService _tokenService;

    public LogoutUserCommandHandler(IJwtTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var token = await _tokenService.GetRefreshToken(request.Token ?? string.Empty, cancellationToken);
        if (token is null)
            throw new UnauthorizedAccessException("Токен не найден");
        await _tokenService.RevokeRefreshToken(token, cancellationToken);
    }
}
