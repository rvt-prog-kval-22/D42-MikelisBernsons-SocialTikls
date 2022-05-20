using BredWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BredWeb.Models
{

    [Index("Title",IsUnique = true)]
    public class EditGroupViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(1500)]
        [MinLength(10)]
        public string? Description { get; set; }
        [Required]
        [StringLength(30)]
        [MinLength(3)]
        public string? Title { get; set; }
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        public string? Creator { get; set; }
        public virtual List<Person>? UserList { get; set; } = new();
        public virtual List<AdminViewModel>? AdminList { get; set; } = new();
        public int UserCount { get; set; }
        public List<Post>? Posts { get; set; } = new();
    }
}
