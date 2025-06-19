using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.Domain.Models.Entities;

namespace User.Domain.Repository;

public class Repository(UserManager<UserEntity> userManager) : IRepository
{
    #region User
    public async Task<UserEntity> AddAsync(UserEntity user, string password)
    {
        var result = userManager.CreateAsync(user, password);

        return user;
    }

    public async Task<bool> EmailExistsAsnyc(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        
        return user != null;
    }

    public async Task<bool> UsernameExistsAsnyc(string username)
    {
        var user = await userManager.FindByNameAsync(username);

        return user != null;
    }

    public async Task<UserEntity> GetByUsernameAsync(string username)
    {
        return await userManager.FindByNameAsync(username);
    }

    public async Task<UserEntity> GetUserByIdAsync(int id)
    {
        return await userManager.FindByIdAsync(id.ToString());
    }

    public async Task<IList<string>> GetRolesAsync(UserEntity user)
    {
        return await userManager.GetRolesAsync(user);
    }
    #endregion
}
