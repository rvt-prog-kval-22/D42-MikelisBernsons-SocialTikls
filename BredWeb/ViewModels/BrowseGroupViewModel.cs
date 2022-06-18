using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class BrowseGroupViewModel
    {
        public List<Post>? Posts { get; set; } = new();
        public Group Group { get; set; } = new();
        public bool IsAdmin { get; set; } = false;
        public string Filter { get; set; } = "";
        public bool Popular { get; set; } = false;
        public List<Rating> UserRatings { get; set; } = new();
    }
}