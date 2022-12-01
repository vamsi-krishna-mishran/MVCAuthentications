using MVCApplicationAlongWithWebAPI.Data;
using System.ComponentModel.DataAnnotations;

namespace MVCApplicationAlongWithWebAPI.Models
{
    public class Book
    {
        [Required]//it makes the field non empty
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Author { get; set; }
        public Book(int id,string title, string description, string author)
        {
            Id = id;
            Title = title;
            Description = description;
            Author = author;
        }

        public static implicit operator Book(Books v)
        {
            throw new NotImplementedException();
        }
    }
}
