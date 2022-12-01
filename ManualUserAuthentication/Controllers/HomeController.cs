using Microsoft.AspNetCore.Mvc;

namespace ManualUserAuthentication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LogIn()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
        public IActionResult Index2()
        {
            return View();
        }
    }
}
