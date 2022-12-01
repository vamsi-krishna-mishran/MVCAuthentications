using Microsoft.AspNetCore.Mvc;
using MVCApplicationAlongWithWebAPI.Data;
using MVCApplicationAlongWithWebAPI.Models;
using System.Diagnostics;

using MVCApplicationAlongWithWebAPI.Repository;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Text;
using Twilio.Clients;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using System.Security.Claims;

namespace MVCApplicationAlongWithWebAPI.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBooksRepository _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly ITwilioRestClient _client;


        // public List<Books> books = new List<Books>();
        public HomeController(ITwilioRestClient client,ILogger<HomeController> logger,IBooksRepository context, IHttpClientFactory clientFactory,IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _clientFactory=clientFactory;
           _config= config;
            _client = client;
            //books.Add(new Books(1, "Ice and Fire", "Alonso Quixano, a retired country gentleman in his fifties, lives in an unnamed section of La Mancha with his niece and a housekeeper. He has become obsessed with books of chivalry, and believes th..", "Jon Snow"));
            //books.Add(new Books(2, "Secret", "Ulysses chronicles the passage of Leopold Bloom through Dublin during an ordinary day, June 16, 1904. The title parallels and alludes to Odysseus (Latinised into Ulysses), the hero of Homer's Odyss..", "socraties"));
            //books.Add(new Books(3, "Wings of fire", "One of the 20th century's enduring works, One Hundred Years of Solitude is a widely beloved and acclaimed novel known throughout the world, and the ultimate achievement in a Nobel Prize–winning car...", "Abdul Kalam Sir"));
            //books.Add(new Books(4, "Secret", "First published in 1851, Melville's masterpiece is, in Elizabeth Hardwick's words, \"the greatest novel in American literature.\" The saga of Captain Ahab and his monomaniacal pursuit of the white wh...", "socraties"));
            
        }
        public ViewResult NotFound()
        {
            return View();
        }
        
        public ActionResult AddBook(int isSuccess=0)
        {
            var result = HttpContext.Session.GetString("_UserName");
            if (HttpContext.Session.GetString("_UserName") == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            else
            {
                ViewBag.status = isSuccess;
                return View();
            }
            
            
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddBook([FromBody]Books book)
        {
            if (HttpContext.Session.GetString("_UserName") == null)
            {
                return View("LogIn", "Account");
            }
            var request = Request;
            if (!ModelState.IsValid)
                return View();
            string token = Request.Headers.Authorization.ToString();
            Console.WriteLine(token);
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(_config["AppUrl"]);
            client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer",token.Substring(7));
            var response = client.PostAsJsonAsync("Books/PostBook", book).Result;
            if (response.IsSuccessStatusCode)
                return RedirectToAction("AddBook", new { isSuccess = 1 });
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("LogIn", "Account");
            else
                return RedirectToAction("AddBook", new { isSuccess = -1 });
        }
        
        public IActionResult AllBooks(List<Books> books=null)
        {
           // if (books == null)
               // return RedirectToAction("LogIn", "Account");
            return View();
        }
        //[Authorize]
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AllBooks()
        {
            try
            {
                var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                string? name=claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
                Console.WriteLine(name);
                //List<Books> obj = JsonConvert.DeserializeObject((string)TempData["mydata"]);

                //List<Books> books = _context.GetBooks();
                //1//JArray? array = (JArray?)JsonConvert.DeserializeObject((string)TempData["mydata"]);
                //2//List<Books>? books = array?.ToObject<List<Books>>();
                string token = Request.Headers.Authorization.ToString();
                Console.WriteLine(token);
                var client = _clientFactory.CreateClient();
                client.BaseAddress = new Uri(_config["AppUrl"]);
                client.DefaultRequestHeaders.Authorization
                             = new AuthenticationHeaderValue("Bearer", token.Substring(7));
                HttpResponseMessage response =await client.GetAsync("Books/GetBooks");
                List<Books> books=new List<Books>();
                if (response.IsSuccessStatusCode)
                {
                    var EmpResponse = response.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    books = JsonConvert.DeserializeObject<List<Books>>(EmpResponse);
                    return View("AllBooks", books);
                   // return View(books);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("LogIn", "Account");
                else
                {
                    Console.Write("something went wrong ..checko ut");
                    return RedirectToAction("Index", new { isSuccess = -1 });
                }
                    
            }
            catch (Exception e)
            {
                Console.Write("error occured in allbooks action method");
                return RedirectToAction("LogIn", "Account");
            }
        }
        public ViewResult Book(int id)
        {
            Books resultbook = null;
            List<Books> books = _context.GetBooks();//getting all books from database
            try
            {
                foreach (Books book in books)//checking for Id
                {
                    if (book.Id == id)//if it true, then a book is exist with  given Id.
                    {
                        resultbook = book;
                        break;
                    }
                }
                if(resultbook== null) { throw new Exception("Not Found"); }
                return View(resultbook);//passing books object to view.
            }
            catch(Exception e)
            {
                return View("NotFound");
            }
            
        }
       [Authorize]
        public IActionResult Index(string? result=null)
        {
            ViewBag.result = result;
            return View();
        }
        public ViewResult GetView()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View("../../TempViews/OutSide");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Index2()
        {
            return View();
        }
        public IActionResult SendOTP()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendOTP(IFormCollection obj)
        {

            var message = MessageResource.Create(
            to: new PhoneNumber(obj["phno"]),
            from: new PhoneNumber("8247731449"),
            body: "6547 is your otp valid for 10 min only..",
            client: _client); // pass in the custom client
            return Ok(message.ToString());
        }
    }
}