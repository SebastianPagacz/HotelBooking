using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Query.UserQueries;

public record ResetPasswordQuery : IRequest<bool>
{
    public string Email { get; set; } = string.Empty;
}
