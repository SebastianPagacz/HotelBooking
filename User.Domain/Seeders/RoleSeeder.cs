using User.Domain.Models.Entities;
using User.Domain.Repository;

namespace User.Domain.Seeders;

public class RoleSeeder(DataContext context) : IRoleSeeder
{
    public async Task SeedAsync()
    {
        if (!context.Roles.Any())
        {
            var roles = new List<Role>
            {
                new Role { Name = "Admin" },
                new Role { Name = "Employee" },
                new Role { Name = "Customer" },
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }
    }
}
