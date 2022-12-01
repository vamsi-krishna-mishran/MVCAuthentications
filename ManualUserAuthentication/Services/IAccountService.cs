using ManualUserAuthentication.Context;
using ManualUserAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ManualUserAuthentication.Services
{
    public interface IAccountService
    {
        public int LogInAsync(string userName,string password);
    }
    public class AccountService : IAccountService
    {
        private readonly BooksContext _booksContext;
        public AccountService(BooksContext booksContext)
        {
            _booksContext = booksContext;
        }
        public  int LogInAsync(string userName,string password)
        {
            Users? result = _booksContext.Users.Where<Users>(user => user.Email ==userName).FirstOrDefault<Users>();
            if (result == null)
                return -1;
            else if (result.Password != password)
                return 0;
            else
                return 1;
        }
    }
}
