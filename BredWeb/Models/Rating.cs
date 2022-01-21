using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        //public List<Post> PostList { get; set;}
        //public List<Comment> CommentList { get; set; }
        //public List<Person> PersonList { get; set; }
        public Status UpvoteStatus { get; set; }
        public enum Status
        {
            Upvoted,
            Downvoted,
            Nothing
        }
    }
}
