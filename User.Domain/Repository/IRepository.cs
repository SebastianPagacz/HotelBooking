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
    public Task<UserEntity> AddAsync(UserEntity user, string password);
    public Task<bool> EmailExistsAsnyc(string email);
    public Task<bool> UsernameExistsAsnyc(string username);
    public Task<UserEntity> GetByUsernameAsync(string username);
    public Task<UserEntity> GetUserByIdAsync(int id);
    public Task<IList<string>> GetRolesAsync(UserEntity user);
    #endregion
}
