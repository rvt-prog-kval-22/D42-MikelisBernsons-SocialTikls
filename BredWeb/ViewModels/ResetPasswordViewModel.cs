using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password must match confirm password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
