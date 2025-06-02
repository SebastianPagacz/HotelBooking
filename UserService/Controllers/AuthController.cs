using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using User.Application.Command.UserCommand;
using User.Application.Query;
using User.Domain.Models.Entities;
using User.Domain.Models.Request;
using User.Domain.Repository;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<UserEntity>> Register(RegisterDTO request)
    {
        var registerDto = await mediator.Send(new RegisterUserCommand
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password
        });
        
        return StatusCode(200, registerDto);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(User.Domain.Models.Request.LoginRequest request)
    {
        string token = await mediator.Send(new LoginUserQuery
        {
            Username = request.Username,
            Password = request.Password
        });

        return token;
    }

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult AdminPage()
    {
        return Ok("Dane tylko dla administratora");
    }
}
