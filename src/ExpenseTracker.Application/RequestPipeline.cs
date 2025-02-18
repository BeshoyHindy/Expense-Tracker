using ExpenseTracker.Application.Common;
using Microsoft.AspNetCore.Builder;

namespace ExpenseTracker.Application;

public static class RequestPipeline
{
    public static IApplicationBuilder UseApplication(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}