using AuthDemoApp.Services;

namespace AuthDemoApp.Middleware
{
    public class SessionAuthMiddleware
    {
        private readonly RequestDelegate _next;
        public SessionAuthMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, ISessionService sessionService)
        {
            var sessionId = context.Request.Cookies["session_id"];
            var session = await sessionService.GetValidSessionAsync(sessionId);

            if (session != null)
                context.Items["CurrentUser"] = session.User;

            await _next(context);
        }
    }

    public static class SessionAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseSessionAuth(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SessionAuthMiddleware>();
        }
    }
}
