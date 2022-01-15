using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Body { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        public bool IsEdited { get; set; } = false;
        public string AuthorName { get; set; }
        public List<Rating> RatingList { get; set; }
        public int Rating { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
