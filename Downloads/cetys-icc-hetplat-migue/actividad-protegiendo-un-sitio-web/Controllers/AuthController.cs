using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthDemoApp.Data;
using AuthDemoApp.Models;
using AuthDemoApp.Services;

namespace AuthDemoApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IPasswordService _passwords;
        private readonly ISessionService _sessions;

        public AuthController(AppDbContext db, IPasswordService passwords, ISessionService sessions)
        {
            _db = db;
            _passwords = passwords;
            _sessions = sessions;
        }

        [HttpGet] public IActionResult Register() => View();
        [HttpGet] public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string Username, string Email, string FullName, DateTime BirthDate, string Password)
        {
            if (await _db.Users.AnyAsync(u => u.Username == Username || u.Email == Email))
            {
                ModelState.AddModelError("", "Usuario o correo ya existen.");
                return View();
            }

            _passwords.CreatePasswordHash(Password, out var hash, out var salt);
            var user = new User { Username = Username, Email = Email, FullName = FullName, BirthDate = BirthDate, PasswordHash = hash, PasswordSalt = salt };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var session = await _sessions.CreateSessionAsync(user.Id);
            Response.Cookies.Append("session_id", session.SessionId, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
            return RedirectToAction("Me", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Username, string Password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == Username);
            if (user == null || !_passwords.VerifyPassword(Password, user.PasswordHash, user.PasswordSalt))
            {
                ModelState.AddModelError("", "Credenciales inválidas.");
                return View();
            }

            var session = await _sessions.CreateSessionAsync(user.Id);
            Response.Cookies.Append("session_id", session.SessionId, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
            return RedirectToAction("Me", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var sid = Request.Cookies["session_id"];
            if (sid != null)
            {
                await _sessions.InvalidateSessionAsync(sid);
                Response.Cookies.Delete("session_id");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
