namespace Recruitment.Web.Middleware.AuditTrailMiddleware
{
    public class AuditTrailMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditTrailMiddleware> _logger;

        public AuditTrailMiddleware(RequestDelegate next, ILogger<AuditTrailMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User?.Identity?.Name ?? "Anonymous";
            var path = context.Request.Path;
            var method = context.Request.Method;

            _logger.LogInformation("[AUDIT TRAIL] User: {User}, Method: {Method}, Path: {Path}, Time: {Time}",
                user, method, path, DateTime.UtcNow);

            await _next(context);
        }
    }

    public static class AuditTrailMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuditTrail(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AuditTrailMiddleware>();
        }
    }
}
