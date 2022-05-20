using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class Admin
    {
        public int Id { get; set; }
        [Required]
        public string AdminId { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
