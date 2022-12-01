using ManualUserAuthentication.Context;
using Microsoft.EntityFrameworkCore;

namespace ManualUserAuthentication.Services
{
    public interface IBookService
    {
        public List<Books> GetBooks();
    }
    public class BookService : IBookService
    {
        public BooksContext _context;
        public BookService(BooksContext context) { _context = context; }

        public  List<Books> GetBooks()
        {
            
            try
            {
                List<Books> result = _context.Books.ToList<Books>();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }


        }
    }
}
