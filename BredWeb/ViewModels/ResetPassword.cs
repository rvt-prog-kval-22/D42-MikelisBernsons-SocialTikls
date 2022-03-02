using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class ResetPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password must match confirm password")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
