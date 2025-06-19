using MediatR;

namespace User.Application.Query.UserQueries;

public record ResetPasswordQuery : IRequest<string>
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
