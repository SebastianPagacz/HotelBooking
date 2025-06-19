using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Application.Query.UserQueries;
using User.Domain.Models.Entities;
using User.Domain.Models.Request;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id}")]
    [Authorize(Policy = "EmployeeOnly")]
    public async Task<IActionResult> GetUser(int id)
    {
        var result = await mediator.Send(new GetUserQuery { Id = id });

        return StatusCode(200, result);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var result = await mediator.Send(new ForgotPasswordQuery { Email = email });

        return StatusCode(200, result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto request)
    {
        var result = await mediator.Send(new ResetPasswordQuery
        {
            Email = request.Email,
            Password = request.NewPassword,
            Token = request.Token
        });

        return StatusCode(200, result);
    }
}
