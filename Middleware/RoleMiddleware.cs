public class RoleMiddleware
{
    private readonly RequestDelegate _next;

    public RoleMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var role = context.Request.Headers["X-Role"].FirstOrDefault();

        if (context.Request.Path.StartsWithSegments("/appointments/admin") && role != "Admin")
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access denied: only Admins can access this endpoint.");
            return;
        }

        await _next(context);
    }
}
