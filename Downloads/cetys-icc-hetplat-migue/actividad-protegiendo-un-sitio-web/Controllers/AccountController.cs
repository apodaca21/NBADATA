using Microsoft.AspNetCore.Mvc;
using AuthDemoApp.Models;

namespace AuthDemoApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Me()
        {
            var user = HttpContext.Items["CurrentUser"] as User;
            if (user == null)
                return RedirectToAction("Login", "Auth");

            return View(user);
        }
    }
}
