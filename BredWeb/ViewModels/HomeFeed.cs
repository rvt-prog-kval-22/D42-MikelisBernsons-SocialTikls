using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class HomeFeed
    {
        public List<Group> Groups { get; set; } = new();
        public List<Post> Posts { get; set; } = new();
    }
}
