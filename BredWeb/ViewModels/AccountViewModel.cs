using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class AccountViewModel
    {
        public List<Post>? Posts { get; set; } = new();
        public Person? Person { get; set; }
        public Statistics Statistics { get; set; } = new();
    }
}
