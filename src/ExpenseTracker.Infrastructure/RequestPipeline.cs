using ExpenseTracker.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;

namespace ExpenseTracker.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();
        return app;
    }
}