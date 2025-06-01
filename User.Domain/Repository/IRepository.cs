using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models.Entities;
using User.Domain.Models.Request;

namespace User.Domain.Repository;

public interface IRepository
{
    #region User
    public Task<UserEntity> AddAsync(UserEntity user);
    public Task<bool> EmailExistsAsnyc(string email);
    public Task<bool> UsernameExistsAsnyc(string username);
    public Task<UserEntity> GetByUsernameAsync(string username);
    #endregion

    #region Role
    public Task<Role> GetRoleByNameAsync(string name);
    #endregion
}
