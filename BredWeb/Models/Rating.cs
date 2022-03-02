using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }
        public int RatedItemId { get; set; }

        public Status Value { get; set; }
        public enum Status
        {
            Upvoted = 1,
            Downvoted = -1,
            Nothing = 0
        }
    }
}
