namespace Manisai_PL_App.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var start = DateTime.UtcNow;

            _logger.LogInformation("Incoming request: {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await _next(context);

            var elapsed = DateTime.UtcNow - start;
            _logger.LogInformation("Completed {Method} {Path} with status {StatusCode} in {Elapsed}ms",
                context.Request.Method, context.Request.Path,
                context.Response.StatusCode, elapsed.TotalMilliseconds);
        }
    }
}