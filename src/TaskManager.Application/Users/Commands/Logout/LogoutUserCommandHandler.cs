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
        await _tokenService.RevokeRefreshToken(request.Token, cancellationToken);
    }
}
