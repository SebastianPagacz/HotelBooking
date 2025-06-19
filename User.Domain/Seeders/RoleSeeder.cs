using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using User.Domain.Models.Entities;
using User.Domain.Repository;

namespace User.Domain.Seeders;

public static class RoleSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

        string[] roles = ["Admin", "Employee", "Customer"];

        foreach (var role in roles) 
        {
            if (!await roleManager.RoleExistsAsync(role)) 
            {
                await roleManager.CreateAsync(new Role { Name = role });
            }
        }
    }
}
