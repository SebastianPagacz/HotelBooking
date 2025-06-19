using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Exceptions;
using User.Domain.Models.DTOs;
using User.Domain.Models.Entities;
using User.Domain.Repository;

namespace User.Application.Query.UserQueries;

public class ForgotPasswordHandler(UserManager<UserEntity> userManager) : IRequestHandler<ForgotPasswordQuery, ResetPasswordDto>
{
    public async Task<ResetPasswordDto> Handle(ForgotPasswordQuery request, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);

        if (existingUser is null)
            throw new UserAlreadyExistsException(); //TODO: create exception

        var token = await userManager.GeneratePasswordResetTokenAsync(existingUser);

        if (string.IsNullOrEmpty(token))
            throw new UserAlreadyExistsException();

        return new ResetPasswordDto
        {
            Email = request.Email,
            Token = token
        };
    }
}
