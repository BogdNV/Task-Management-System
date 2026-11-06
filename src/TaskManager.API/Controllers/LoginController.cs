using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Models.Response;
using TaskManager.API.Models.Request;
using TaskManager.Application.Users.Commands.Login;
using TaskManager.Application.Users.Commands.Logout;
using TaskManager.Application.Users.DTO;
using TaskManager.Application.Users.Commands.Refresh;
using Microsoft.AspNetCore.Authorization;
using TaskManager.Abstractions.Inrefaces;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class LoginController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IGetUserIdService _idService;

    public LoginController(IMediator mediator, IMapper mapper, IGetUserIdService idService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _idService = idService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest login, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<LoginUserCommand>(login);

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse<LoginResponse>()
        {
            Data = _mapper.Map<LoginResponse>(result),
            Message = "Success"
        });
    }


    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<RefreshTokenCommandResult>>> Refresh([FromBody] TokenDto token, CancellationToken ct)
    {
        var command = new RefreshTokenCommand { RefreshToken = token.RefreshToken };
        var result = await _mediator.Send(command, ct);

        return new ApiResponse<RefreshTokenCommandResult>
        {
            Data = result,
            Message = "Success"
        };
    }


    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] TokenDto token, CancellationToken ct)
    {
        await _mediator.Send(new LogoutUserCommand { UserId = _idService.Id, Token = token.RefreshToken }, ct);
        return Ok();
    }
}
