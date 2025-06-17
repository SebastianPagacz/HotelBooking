using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models.Entities;

namespace User.Application.Query.UserQueries;

public record GetUserQuery : IRequest<UserEntity>
{
    public int Id { get; set; }
}
