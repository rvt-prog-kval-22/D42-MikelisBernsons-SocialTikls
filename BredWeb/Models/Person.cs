using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class Person : IdentityUser
    {
        [Required]
        [MaxLength(20)]
        [DisplayName("NickName")]
        public string NickName { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
        public DateTime DateCreated { get; set; }
        public List<Group> GroupUserList { get; set; }
        public List<Rating> RatingList { get; set; }
    }
}
