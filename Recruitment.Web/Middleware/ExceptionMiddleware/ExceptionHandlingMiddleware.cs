using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Recruitment.Web.Models;
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
            if (context.Request.Path.StartsWithSegments("/Home/ErrorModal"))
            {
                await _next(context);
                return;
            }

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                bool isAjax =
                    context.Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                    context.Request.Headers["Accept"].Any(a => a.Contains("application/json")) ||
                    context.Request.Path.StartsWithSegments("/api");

                if (isAjax)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 500;

                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        success = false,
                        message = ex.Message,
                        traceId = context.TraceIdentifier
                    }));
                    return;
                }

                var tempDataFactory =
                    context.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();

                var tempData = tempDataFactory.GetTempData(context);

                tempData["ErrorMessage"] = "Something went wrong";
                tempData["ErrorDetails"] = _env.IsDevelopment() ? ex.Message : null;
                tempData["TraceId"] = context.TraceIdentifier;

                context.Request.Path = "/Home/ErrorModal";
                await _next(context);
            }
        }

    }
}

