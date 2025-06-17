using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Exceptions;
using User.Domain.Repository;

namespace User.Application.Query.UserQueries;

public class ResetPasswordHandler(IRepository repository) : IRequestHandler<ResetPasswordQuery, bool>
{
    public async Task<bool> Handle(ResetPasswordQuery request, CancellationToken cancellationToken)
    {
        var exisitngEmail = await repository.EmailExistsAsnyc(request.Email);

        if (exisitngEmail is false)
            throw new UserNotFoundException();

        throw new NotImplementedException();
    }
}
