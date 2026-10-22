using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Models.Request.User;
using TaskManager.Application.Users.Commands;

namespace TaskManager.API.Models.Extensions;

public static class RegisterMapper
{
    public static RegisterUserCommand ToCommand(this RegisterRequest request)
    {
        return new RegisterUserCommand()
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        };
    }
}
