
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.Domain.Models.Entities;
using User.Domain.Repository;
using UserService.Middleware;

namespace UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseInMemoryDatabase("UserDb"));
            builder.Services.AddScoped<IRepository, Repository>();
            builder.Services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(User.Application.AssemblyReference).Assembly));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
