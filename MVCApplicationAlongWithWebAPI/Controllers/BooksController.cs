using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCApplicationAlongWithWebAPI.Data;
using MVCApplicationAlongWithWebAPI.Repository;
using Newtonsoft.Json;
using System.Security.Claims;
using Twilio.TwiML.Voice;

namespace WebAPI.Controllers
{
    [Route("[controller]/[action]")]//route specification
    [ApiController]//make the class as api controller
    //[Authorize]
    [Authorize(Roles = "Administrator")]
    public class BooksController : ControllerBase//inheriting controllerbase
    {
        private readonly IBooksRepository _context;//our database context class <BooksContext>
        public BooksController(IBooksRepository context)//constructor to instantiate the context.
        {
            _context = context;//getting instance of dbcontext class .
        }
        [HttpGet]//to get the data
        [Authorize]
        public IActionResult GetBooks()
        {
            //var records = _context.Books.ToList<Books>();//returns the list of allbooks
            
            try
            {
                List<Books> records = _context.GetBooks();
                if (records == null) return NotFound(JsonConvert.SerializeObject(new { status = 0, data = "" }));
                return Ok(JsonConvert.SerializeObject(new {status=1,data=records}));    
            }
            catch(Exception ex)
            {
                return NotFound(JsonConvert.SerializeObject(new { status = -1, data ="" }));
            }
            //TempData["mydata"] = JsonConvert.SerializeObject(records);
            //return RedirectToAction("AllBooks", "Home");
            // return Ok(records);

        }
        
        [HttpPost]
        [Authorize]
        public IActionResult PostBook(Books book)
        {
            
            if (!ModelState.IsValid)
                 return BadRequest();
            int status = _context.PostBook(book);
            if(status==1)
             {
                return Ok("uploaded successfullly");
             }
            else if (status == 0)
            {
                return BadRequest();
            }
            else
            {
                return BadRequest();
            }
           
        }
        [HttpDelete]
        public IActionResult DeleteBook([FromQuery] string bookName)
        {
            /*var existance = _context.Books.Where<Books>(b => b.Title == bookName).FirstOrDefault<Books>();
            //returns first match of bookName if not returns default which is null.
            if (existance == null)
                return NotFound();
            //_context.Entry(Books).State = System.Data.Entity.EntityState.Deleted;
            _context.Books.Remove(existance);//it removes the instance from context.
            _context.SaveChanges();//saving the changes in the database
            return Ok(bookName);*/
            if (_context.DeleteBook(bookName) == 0) return NotFound();
            return Ok(bookName);
            
        }
        [HttpPut]
        public IActionResult UpdateBook([FromQuery] string bookName, [FromForm] Books book)
        {
            /*if (!ModelState.IsValid)//if model is not validated..it executes.
                return BadRequest();//returns 400 status saying bad request
            else
            {
                var existance = _context.Books.Where<Books>(instance=>instance.Title==bookName).FirstOrDefault<Books>();
                if (existance != null)
                {
                    existance.Title = bookName;//updating instance values..
                    existance.Description = book.Description;
                    existance.Author = book.Author;
                    _context.SaveChanges();//saving the changes 
                    return Ok(existance);
                }
                else
                {
                    return NotFound();
                }
            }*/
            if (!ModelState.IsValid)//if model is not validated..it executes.
                return BadRequest();//returns 400 status saying bad request
            else if (_context.UpdateBook(bookName, book) == 1) return Ok("success");
            return NotFound("failure");
        }
    }
}
