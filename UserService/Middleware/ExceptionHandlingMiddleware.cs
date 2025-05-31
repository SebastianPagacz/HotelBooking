using User.Domain.Exceptions;

namespace UserService.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (UserNotFoundException ex) 
        {
            logger.LogWarning(ex, ex.Message);
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new {error = ex.Message});
        }
        catch (UserAlreadyExistsException ex)
        {
            logger.LogWarning(ex, ex.Message);
            context.Response.StatusCode = 406;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, ex.Message);
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
    }
}
