using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class Admin
    {
        public int Id { get; set; }
        [Required]
        public string AdminId { get; set; }
        public bool IsSelected { get; set; } = false;
        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
