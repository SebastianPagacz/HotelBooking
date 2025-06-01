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

public class RegisterUserHandler(IRepository repository, IPasswordHasher<UserEntity> hasher) : IRequestHandler<RegisterUserCommand, RegisterDTO>
{
    public async Task<RegisterDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUsername = await repository.UsernameExistsAsnyc(request.Username);
        var existingEmail = await repository.UsernameExistsAsnyc(request.Email);

        if (existingUsername is true || existingEmail is true)
            throw new UserAlreadyExistsException();

        // TODO: Change it
        var customerRole = await repository.GetRoleByNameAsync("Customer");
        var roles = new List<Role>();
        roles.Add(customerRole);

        var user = new UserEntity
        {
            Username = request.Username,
            Email = request.Email,
            CreatedAt = DateTime.Now,
            IsDeleted = false,
            Roles = roles
        };

        var hashedPassword = hasher.HashPassword(user, request.Password);

        user.PasswordHash = hashedPassword;

        await repository.AddAsync(user);

        return new RegisterDTO { Username = user.Username, Email = user.Email, Password = user.PasswordHash};
    }
}
