using MediatR;
using TaskManager.Abstractions.Inrefaces;
using TaskManager.Application.Users.DTO;

namespace TaskManager.Application.Users.Commands;

public class TokenCookieBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : ITokenResult
    where TRequest : ICommandWithCookie
{
    private readonly IJwtTokenService _tokenService;

    public TokenCookieBehavior(IJwtTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next(cancellationToken);
        if (request.SaveToCookie)
            _tokenService.SetTokenInsideCookie(response.AccessToken, response.RefreshToken);
        return response;
    }
}
