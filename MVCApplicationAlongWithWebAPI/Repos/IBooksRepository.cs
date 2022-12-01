using MVCApplicationAlongWithWebAPI.Data;
namespace MVCApplicationAlongWithWebAPI.Repository
{
    public interface IBooksRepository
    {
        public List<Books> GetBooks();
        public int PostBook(Books book);
        public int DeleteBook(string bookName);
        public int UpdateBook(string bookName,Books book);
    }
    public class BooksRepository : IBooksRepository
    {

        private readonly BooksContext _context;
        public BooksRepository(BooksContext context)
        {
            _context = context;
        }
        public List<Books> GetBooks()
        {
            return _context.Books.ToList<Books>();
        }
        public async Task<Books> GetBook(int Id)
        {
            return await _context.Books.FindAsync(Id);
        }
        public int PostBook(Books book)
        {
            try
            {
                book.Id = null;
                //book.Id = book.Id.HasValue ? book.Id.Value : 0;

                _context.Books.Add(book);
                _context.SaveChanges();
                return 1;
            }
            catch(Exception e)
            {
                return 0;
            }
        }
        public int DeleteBook(string bookName)
        {
            var existance = _context.Books.Where<Books>(b => b.Title == bookName).FirstOrDefault<Books>();
            //returns first match of bookName if not returns default which is null.
            if (existance == null)
                return 0;
            //_context.Entry(Books).State = System.Data.Entity.EntityState.Deleted;
            _context.Books.Remove(existance);//it removes the instance from context.
            _context.SaveChanges();//saving the changes in the database
            return 1;
        }
        public int UpdateBook(string bookName, Books book)
        {
            var existance = _context.Books.Where<Books>(instance => instance.Title == bookName).FirstOrDefault<Books>();
            if (existance != null)
            {
                existance.Title = book.Title;//updating instance values..
                existance.Description = book.Description;
                existance.Author = book.Author;
                _context.SaveChanges();//saving the changes 
                return 1;
            }
            else
            {
                return 0;
            }


        }
    }
}
