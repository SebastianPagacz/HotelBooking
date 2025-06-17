using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Application.Query.UserQueries;
using User.Domain.Models.Entities;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id}")]
    [Authorize(Policy = "EmployeeOnly")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await mediator.Send(new GetUserQuery { Id = id });

        return StatusCode(200, result);
    }

    [HttpPost("reset-email")]
    public async Task<IActionResult> ResetPassword(string email)
    {

    }
}
