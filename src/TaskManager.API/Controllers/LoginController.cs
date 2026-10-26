using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Models.Response;
using TaskManager.API.Models.Request;
using TaskManager.Application.Users.Commands.Login;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class LoginController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public LoginController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest login, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<LoginUserCommand>(login);

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse<LoginResponse>()
        {
            Data = _mapper.Map<LoginResponse>(result),
            Message = "Done"
        });
    }
}
