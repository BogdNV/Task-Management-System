using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Abstractions.Inrefaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(Guid id, string name);
}
