using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using User.Application.Services;
using User.Domain.Exceptions;
using User.Domain.Models.Entities;
using User.Domain.Models.Request;
using User.Domain.Repository;

namespace User.Application.Query;

public class LoginUserHandler(IRepository repository, IPasswordHasher<UserEntity> hasher, IJWTTokenService tokenService) : IRequestHandler<LoginUserQuery, string>
{
    public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var exisitingUser = await repository.GetByUsernameAsync(request.Username);

        if (exisitingUser is null || exisitingUser.IsDeleted is true)
            throw new UserNotFoundException();

        if (hasher.VerifyHashedPassword(exisitingUser, exisitingUser.PasswordHash, request.Password)
            == PasswordVerificationResult.Failed)
            throw new UserNotFoundException();

        var roles = exisitingUser.Roles.Select(r => r.Name).ToList();

        string token = tokenService.GenerateToken(exisitingUser.Id, roles);

        return token;
    }
}