using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("Nickname")]
        public string NickName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("Email")]
        public string EmailAddress { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
