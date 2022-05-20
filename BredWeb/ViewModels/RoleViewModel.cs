using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class RoleViewModel
    {
        [Required]
        [MinLength(3)]
        public string RoleName { get; set; } = string.Empty;
    }
}
