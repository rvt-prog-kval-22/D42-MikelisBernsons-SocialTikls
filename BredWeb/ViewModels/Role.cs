using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class Role
    {
        [Required]
        public string RoleName { get; set; }
    }
}
