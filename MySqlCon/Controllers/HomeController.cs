using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlCon.Models;
using MySqlConnector;
using System.Diagnostics;

namespace MySqlCon.Controllers
{
    //[AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;   
        }
        //private static void InsertData()
        //{
        //    using (var context = new LibraryContext())
        //    {
        //        // Creates the database if not exists
        //        context.Database.EnsureCreated();

        //        // Adds a publisher
        //        var publisher = new Publisher
        //        {
        //            Name = "Mariner Books"
        //        };
        //        context.Publisher.Add(publisher);

        //        // Adds some books
        //        context.Book.Add(new Book
        //        {
        //            ISBN = "978-0544003415",
        //            Title = "The Lord of the Rings",
        //            Author = "J.R.R. Tolkien",
        //            Language = "English",
        //            Pages = 1216,
        //            Publisher = publisher
        //        });
        //        context.Book.Add(new Book
        //        {
        //            ISBN = "978-0547247762",
        //            Title = "The Sealed Letter",
        //            Author = "Emma Donoghue",
        //            Language = "English",
        //            Pages = 416,
        //            Publisher = publisher
        //        });

        //        // Saves changes
        //        context.SaveChanges();
        //    }
        //}
        //private static void PrintData()
        //{
        //    // Gets and prints all books in database
        //    using (var context = new LibraryContext())
        //    {
        //        var books = context.Book
        //          .Include(p => p.Publisher);
        //        foreach (var book in books)
        //        {
        //            var data = new StringBuilder();
        //            data.AppendLine($"ISBN: {book.ISBN}");
        //            data.AppendLine($"Title: {book.Title}");
        //            data.AppendLine($"Publisher: {book.Publisher.Name}");
        //            Console.WriteLine(data.ToString());
        //        }
        //    }
        //}
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Index()
        {
            //InsertData();
            // PrintData();
            var request = Request.HttpContext.User;
            using var connection = new MySqlConnection(_config["ConnectionStrings:Default"]);
            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT * FROM Books;", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var value = reader.GetValue(0);
                // do something with 'value'
                Console.WriteLine(value);
            }

            return View();
        }
        // [Authorize]
        [AllowAnonymous]
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