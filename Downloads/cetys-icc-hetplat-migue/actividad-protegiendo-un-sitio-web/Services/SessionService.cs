using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using AuthDemoApp.Data;
using AuthDemoApp.Models;

namespace AuthDemoApp.Services
{
    public class SessionService : ISessionService
    {
        private readonly AppDbContext _db;
        private readonly TimeSpan _idleTimeout = TimeSpan.FromMinutes(5);

        public SessionService(AppDbContext db) => _db = db;

        private string GenerateSessionId()
        {
            byte[] bytes = RandomNumberGenerator.GetBytes(16); // 128 bits
            return Convert.ToHexString(bytes);
        }

        public async Task<Session> CreateSessionAsync(int userId)
        {
            var s = new Session
            {
                SessionId = GenerateSessionId(),
                UserId = userId,
                LastActivityUtc = DateTime.UtcNow,
                IsValid = true
            };
            _db.Sessions.Add(s);
            await _db.SaveChangesAsync();
            return s;
        }

        public async Task<Session?> GetValidSessionAsync(string? sessionId)
        {
            if (string.IsNullOrEmpty(sessionId)) return null;

            var session = await _db.Sessions.Include(s => s.User)
                .FirstOrDefaultAsync(s => s.SessionId == sessionId && s.IsValid);

            if (session == null) return null;

            if (DateTime.UtcNow - session.LastActivityUtc > _idleTimeout)
            {
                session.IsValid = false;
                await _db.SaveChangesAsync();
                return null;
            }

            session.LastActivityUtc = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return session;
        }

        public async Task InvalidateSessionAsync(string sessionId)
        {
            var s = await _db.Sessions.FirstOrDefaultAsync(x => x.SessionId == sessionId);
            if (s != null)
            {
                s.IsValid = false;
                await _db.SaveChangesAsync();
            }
        }
    }
}
