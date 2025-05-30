using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User.Domain.Models.Entities;

namespace User.Domain.Repository;

public class Repository(DataContext context) : IRepository
{
    public async Task<UserEntity> AddAsync(UserEntity user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return user;
    }
}
