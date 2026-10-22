using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    [EmailAddress(ErrorMessage = "Некоректный email")]
    public string Email { get; private set; } = string.Empty;

    public User(string name, string password, string email)
    {
        Name = name;
        Password = password;
        Email = email;
    }

}
