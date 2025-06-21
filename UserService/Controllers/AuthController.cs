using MediatR;
using Microsoft.AspNetCore.Mvc;
using User.Application.Command.UserCommand;
using User.Application.Query;
using User.Domain.Models.Request;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO request)
    {
        var registerDto = await mediator.Send(new RegisterUserCommand
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password
        });
        
        return StatusCode(200, "Succesfully registered!");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(User.Domain.Models.Request.LoginRequest request)
    {
        string token = await mediator.Send(new LoginUserQuery
        {
            Username = request.Username,
            Password = request.Password
        });

        return StatusCode(200, token);
    }
}
