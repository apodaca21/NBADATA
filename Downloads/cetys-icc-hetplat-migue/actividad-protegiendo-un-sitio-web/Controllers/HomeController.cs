using Microsoft.AspNetCore.Mvc;

namespace AuthDemoApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
