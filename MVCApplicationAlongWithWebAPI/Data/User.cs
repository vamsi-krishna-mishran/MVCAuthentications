using System.ComponentModel.DataAnnotations;

namespace MVCApplicationAlongWithWebAPI.Data
{
    public class User
    {
        [Required]
        public int Id { get; set; } 
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
