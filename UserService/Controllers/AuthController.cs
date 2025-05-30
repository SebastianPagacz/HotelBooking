using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using User.Application.Command.UserCommand;
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
}
