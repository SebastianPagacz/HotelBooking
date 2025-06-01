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
    #region User
    public async Task<UserEntity> AddAsync(UserEntity user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return user;
    }

    public async Task<bool> EmailExistsAsnyc(string email)
    {
        return await context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> UsernameExistsAsnyc(string username)
    {
        return await context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<UserEntity> GetByUsernameAsync(string username)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
    #endregion

    #region Role
    public async Task<Role> GetRoleByNameAsync(string name)
    {
        return await context.Roles.FirstOrDefaultAsync(r => r.Name == name);
    }
    #endregion
}
