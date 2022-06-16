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
        [DisplayName("Nickname")]
        public string? NickName { get; set; }
        [Required]
        [DisplayName("Birthday")]
        public DateTime BirthDay { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual List<Group>? GroupUserList { get; set; }
    }
}
