﻿//namespace MySqlPolemro.Models
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySqlPolemro.Models
{
    [Table("owner")]
    public class Owner
    {
        public Guid OwnerId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string? Name { get; set; }
        public string? Description { get; set; }    

       // [Required(ErrorMessage = "Date of birth is required")]
       // public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(100, ErrorMessage = "Address cannot be longer than 100 characters")]
        public string? Address { get; set; }

        public ICollection<Account>? Accounts { get; set; }
       
    }
}