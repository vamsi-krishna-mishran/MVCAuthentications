using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlPolemro.context;
using MySqlPolemro.Models;
using System.Diagnostics;

namespace MySqlPolemro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RepositoryContext _context;

        public HomeController(ILogger<HomeController> logger,RepositoryContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            //List<User> students = _context.Users
            //       .FromSqlRaw("Select * from users")
            //       .ToList<User>();
            User user = new User();user.Id = 4322;
            user.Name = "nameofhere";
            
            _context.Users.Add(user);
            _context.SaveChanges();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}