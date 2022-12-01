using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ManualUserAuthentication.Context
{
    public class BooksContext : IdentityDbContext<ApplicationUser>
    {
        public BooksContext(DbContextOptions<BooksContext> options) : base(options) { }
        public DbSet<Books> Books { get; set; }
        public DbSet<Users> Users { get; set; }
}
    public class Books
    {
        
        public int Id { get; set; }
        [Required]
        public string BookName { get; set; }
        [Required]
        public string AuthorName { get; set; }
    }
    public class Users
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Key]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
