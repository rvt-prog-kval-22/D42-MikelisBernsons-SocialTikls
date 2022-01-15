using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public bool Upvoted { get; set; }
        public string AuthorName { get; set; }
    }
}
