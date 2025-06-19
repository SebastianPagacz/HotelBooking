using MediatR;
using User.Domain.Models.DTOs;

namespace User.Application.Query.UserQueries;

public record ForgotPasswordQuery : IRequest<ResetPasswordDto>
{
    public string Email { get; set; } = string.Empty;
}
