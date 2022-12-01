using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCApplicationAlongWithWebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MVCApplicationAlongWithWebAPI.Data
{
    public class BooksContext : IdentityDbContext<ApplicationUser>// instead of DbContext
    {
        public BooksContext(DbContextOptions<BooksContext> options) : base(options) { }
        public DbSet<Books> Books { get; set; }

    }
    public class Books
    {
        [Required]
        public int? Id { get; set; }
        [Required(ErrorMessage = "please enter title field.")]//custom error message 
        //[Display(Name ="It won't get Into database")]
        //[EmailAddress(ErrorMessage ="title should be email address")]//to make title to be a email addr.
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Img { get; set; }
    }
    [Table("tablewithtableannotator")]
    public class Tab
    {
        public int Id { get; set; } 
        public int TabName { get; set; }
    }
}
