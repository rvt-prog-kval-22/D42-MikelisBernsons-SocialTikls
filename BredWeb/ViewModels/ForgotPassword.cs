using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
