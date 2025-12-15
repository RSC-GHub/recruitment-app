using System.Net;
using System.Text.Json;

namespace Recruitment.Web.Middleware.ExceptionMiddleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                // API request
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.ContentType = "application/json";

                    var result = JsonSerializer.Serialize(new
                    {
                        message = "An unexpected error occurred.",
                        details = _env.IsDevelopment() ? ex.Message : null,
                        traceId = context.TraceIdentifier
                    });

                    await context.Response.WriteAsync(result);
                    return;
                }

                //  MVC request
                context.Items["Exception"] = ex;
                context.Request.Path = "/Home/Error";

                await _next(context);
            }
        }
    }


}
