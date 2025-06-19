using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using User.Domain.Models.Entities;
using User.Domain.Models.Request;
using User.Domain.Repository;
using Microsoft.AspNetCore.Identity;
using User.Domain.Exceptions;

namespace User.Application.Command.UserCommand;

public class RegisterUserHandler(UserManager<UserEntity> userManager, RoleManager<Role> roleManager) : IRequestHandler<RegisterUserCommand, RegisterDTO>
{
    public async Task<RegisterDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUsername = await userManager.FindByNameAsync(request.Username);
        var existingEmail = await userManager.FindByEmailAsync(request.Email);

        var user = new UserEntity
        {
            UserName = request.Username,
            Email = request.Email,
            CreatedAt = DateTime.Now,
            IsDeleted = false,
        };

        var createResult = await userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded) 
        {
            throw new Exception(string.Join("; ", createResult.Errors.Select(e => e.Description)));
        }

        var roleExists = await roleManager.RoleExistsAsync("Admin");
        if (!roleExists)
        {
            await roleManager.CreateAsync(new Role { Name = "Admin" });
        }

        var addToRoleResault = await userManager.AddToRoleAsync(user, "Admin");

        return new RegisterDTO { Username = user.UserName, Email = user.Email, Password = user.PasswordHash};
    }
}
