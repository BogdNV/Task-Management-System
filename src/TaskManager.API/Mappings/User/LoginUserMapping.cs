using AutoMapper;
using TaskManager.API.Models.Request;
using TaskManager.API.Models.Response;
using TaskManager.Application.Users.Commands.Login;

namespace TaskManager.API.Mappings.User;

public class LoginUserMapping : Profile
{
    public LoginUserMapping()
    {
        CreateMap<LoginRequest, LoginUserCommand>();
        CreateMap<LoginUserCommandResult, LoginResponse>();
    }
}
