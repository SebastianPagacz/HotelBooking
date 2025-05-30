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

namespace User.Application.Command.UserCommand;

public class RegisterUserHandler(IRepository repository, IPasswordHasher<UserEntity> hasher) : IRequestHandler<RegisterUserCommand, RegisterDTO>
{
    public async Task<RegisterDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new UserEntity
        {
            Username = request.Username,
            Email = request.Email,
            CreatedAt = DateTime.Now,
            IsDeleted = false
        };

        var hashedPassword = hasher.HashPassword(user, request.Password);

        user.PasswordHash = hashedPassword;

        await repository.AddAsync(user);

        return new RegisterDTO { Username = user.Username, Email = user.Email, Password = user.PasswordHash};
    }
}
