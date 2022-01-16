using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class Register
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password don't match.")]
        public string ConfirmPassword { get; set; }

        //[Required]
        //[MinLength(3)]
        //[MaxLength(18)]        
        //public string NickName { get; set; }        
        //public DateTime BirthDay { get; set; }
    }
}
