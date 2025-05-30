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
    public Task<UserEntity> AddAsync(UserEntity user);
}
