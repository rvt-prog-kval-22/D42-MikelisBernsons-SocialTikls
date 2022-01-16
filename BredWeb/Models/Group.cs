using BredWeb.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(1500)]
        [MinLength(10)]
        public string Description { get; set; }
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string Title { get; set; }
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        public string Creator { get; set; } = "Amogu";
        public List<Person>? UserList { get; set; }
        public List<Person>? AdminList { get; set; }
        public int UserCount { get; set; }
        public List<Post>? Posts { get; set; }
    }
}
