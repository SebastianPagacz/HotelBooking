using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Exceptions;
using User.Domain.Models.Entities;
using User.Domain.Repository;

namespace User.Application.Query.UserQueries;

public class GetUserHandler(IRepository repository) : IRequestHandler<GetUserQuery, UserEntity>
{
    public async Task<UserEntity> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetUserByIdAsync(request.Id) ?? throw new UserNotFoundException();
    }
}
