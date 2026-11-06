using MediatR;
using TaskManager.API.Models.Request;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Commands;
using AutoMapper;
using TaskManager.API.Models.Response;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class RegistrationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public RegistrationController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }


    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterUserCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(new ApiResponse<RegisterResponse>()
        {
            Data = _mapper.Map<RegisterResponse>(result),
            Message = "Success"
        });
    }
}
