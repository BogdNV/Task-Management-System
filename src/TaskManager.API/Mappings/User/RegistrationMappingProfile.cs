using AutoMapper;
using TaskManager.API.Models.Request;
using TaskManager.API.Models.Response;
using TaskManager.Application.Users.Commands;

namespace TaskManager.API.Mappings;

public class RegistrationMappingProfile : Profile
{
    public RegistrationMappingProfile()
    {
        CreateMap<RegisterRequest, RegisterUserCommand>();
        CreateMap<RegisterUserCommandResult, RegisterResponse>();
    }
}
