using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class Register
    {
        [Required]
        [MaxLength(20)]
        [DisplayName("NickName")]
        public string NickName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDay { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password don't match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
