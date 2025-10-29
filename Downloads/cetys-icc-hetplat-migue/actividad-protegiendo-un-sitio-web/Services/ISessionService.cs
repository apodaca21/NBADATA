using AuthDemoApp.Models;

namespace AuthDemoApp.Services
{
    public interface ISessionService
    {
        Task<Session?> GetValidSessionAsync(string? sessionId);
        Task<Session> CreateSessionAsync(int userId);
        Task InvalidateSessionAsync(string sessionId);
    }
}
