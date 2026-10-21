using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Commands;
using TaskManager.Application.Users.DTO;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class RegistrationController : ControllerBase
{
    private readonly IMediator _mediator;

    public RegistrationController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterAsync(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
