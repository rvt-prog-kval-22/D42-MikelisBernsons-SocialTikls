using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class OpenPostViewModel
    {
        public List<Comment>? Comments { get; set; } = new();
        public Group Group { get; set; } = new();
        public Post Post { get; set; } = new();
        public string UserId { get; set; } = "";
        public bool GroupAdmin { get; set; } = false;
        public string UserNick { get; set; } = "";
    }
}
