using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using User.Domain.Models.Entities;
using User.Domain.Models.Request;

namespace User.Application.Command.UserCommand;

public record RegisterUserCommand : IRequest<RegisterDTO>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
