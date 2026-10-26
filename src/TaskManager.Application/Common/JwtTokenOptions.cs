using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Application.Common;

public class JwtTokenOptions
{
    public string SecretKey { get; set; } = string.Empty;
    // public string Issuer { get; set; } = string.Empty;
    // public string Audience { get; set; } = string.Empty;
    public TimeSpan Expires { get; set; }

    public override string ToString() => $"{nameof(JwtTokenOptions)} {{ Secret: {SecretKey} Minute: {Expires}}}";
}
