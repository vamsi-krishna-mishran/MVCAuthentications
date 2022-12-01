using ManualUserAuthentication.Context;
using ManualUserAuthentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Filters;

namespace ManualUserAuthentication.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    //[Authorize]
    
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
       // private readonly HttpActionContext _httpContext;
        public BooksController(IBookService bookService) {
            _bookService = bookService;
           
        }
        //[BasicAuthentication]

        //[Authorize]
       // [TypeFilter(typeof(BasicAuthFilter))]
        [HttpGet]
        public IActionResult GetBooks()
        {
            //throw new ArithmeticException();
            try
            {
                List<Books> obj = _bookService.GetBooks();
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }
        [HttpGet]
        public IActionResult GetBooks2()
        {
            try
            {
                List<Books> obj = _bookService.GetBooks();
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
