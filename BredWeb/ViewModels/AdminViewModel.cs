using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class AdminViewModel
    {
        public int Id { get; set; }
        [Required]
        public string AdminId { get; set; } = string.Empty;
        public bool IsSelected { get; set; } = false;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
