using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class EditRole
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Role name required")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; } = new();
    }
}
