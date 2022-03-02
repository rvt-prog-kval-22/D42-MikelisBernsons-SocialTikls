using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class EditRole
    {
        public string Id { get; set; } = string.Empty;
        [Required(ErrorMessage = "Role name required")]
        [MinLength(3)]
        public string RoleName { get; set; } = string.Empty;
        public List<string> Users { get; set; } = new();
    }
}
