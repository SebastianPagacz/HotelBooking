using MediatR;
using Microsoft.AspNetCore.Identity;
using User.Application.Services;
using User.Domain.Exceptions;
using User.Domain.Models.Entities;
using User.Domain.Repository;

namespace User.Application.Query;

public class LoginUserHandler(UserManager<UserEntity> userManager, IJWTTokenService tokenService) : IRequestHandler<LoginUserQuery, string>
{
    public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByNameAsync(request.Username);

        if (existingUser is null || existingUser.IsDeleted is true)
            throw new InvalidCredentialException();

        var isPasswordValid = await userManager.CheckPasswordAsync(existingUser, request.Password);
        if (!isPasswordValid)
            throw new InvalidCredentialException();

        var roles = await userManager.GetRolesAsync(existingUser);

        string token = tokenService.GenerateToken(existingUser.Id, roles.ToList());

        return token;
    }
}