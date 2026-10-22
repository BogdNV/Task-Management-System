using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Application.Users.Commands;
using TaskManager.Application.Users.DTO;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Application.Mappers;

public static partial class UserMapper
{
    public static User ToDomain(this RegisterUserCommand command)
    {
        return new User(command.Name, command.Password, command.Email);
    }

    public static UserDto ToDto(this UserEntity user)
    {
        return new UserDto { Id = user.Id, Name = user.Name, Email = user.Email };
    }
}
