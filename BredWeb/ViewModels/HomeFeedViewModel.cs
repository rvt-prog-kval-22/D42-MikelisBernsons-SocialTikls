using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class HomeFeedViewModel
    {
        public List<Group> Groups { get; set; } = new();
        public List<Post> Posts { get; set; } = new();
        public List<Rating> UserRatings { get; set; } = new();
    }
}
