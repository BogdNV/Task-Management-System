using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Infrastructure.Mappers;

public static partial class UserMapper
{
    public static UserEntity ToEntity(this User domain)
    {
        return new UserEntity
        {
            Name = domain.Name,
            Password = domain.Password,
            Email = domain.Email
        };
    }
}
