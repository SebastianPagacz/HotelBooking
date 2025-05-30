using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User.Domain.Models.Entities;

namespace User.Domain.Repository;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
}
