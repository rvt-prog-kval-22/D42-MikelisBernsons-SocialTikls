using System.ComponentModel;

namespace BredWeb.Models
{
    public class Statistics
    {

        [DisplayName("Groups created")]
        public int GroupsCreated { get; set; } = 0;
        [DisplayName("Moderated groups")]
        public int ModeratedGroups { get; set; } = 0;
        [DisplayName("Groups joined")]
        public int JoinedGroups { get; set; } = 0;
        [DisplayName("Post count")]
        public int PostCount { get; set; } = 0;
        [DisplayName("Comment count")]
        public int CommentCount { get; set; } = 0;
        [DisplayName("Total rating")]
        public int TotalRating { get; set; } = 0;
    }
}
