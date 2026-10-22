using MediatR;
using TaskManager.API.Models.Request.User;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Commands;
using TaskManager.Application.Users.DTO;
using TaskManager.API.Models.Extensions;

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
    public async Task<ActionResult<UserDto>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
