using MediatR;
using Microsoft.AspNetCore.Identity;
using User.Domain.Exceptions;
using User.Domain.Models.Entities;

namespace User.Application.Query.UserQueries;

public class ResetPasswordHandler(UserManager<UserEntity> userManager) : IRequestHandler<ResetPasswordQuery, string>
{
    public async Task<string> Handle(ResetPasswordQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return "failed";

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (result.Succeeded)
            return "success";

        return "failed";
    }
}
