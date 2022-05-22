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
        [MinLength(4)]
        public string? Body { get; set; }
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string? Title { get; set; }
        [Display(Name = "Post date")]
        public DateTime PostDate { get; set; }
        public bool IsEdited { get; set; } = false;
        [MaxLength(20)]
        [Display(Name = "Authors name")]
        public string? AuthorName { get; set; }
        public List<Rating>? RatingList { get; set; } = new();
        public List<Comment>? CommentList { get; set; } = new();
        public int TotalRating { get; set; } = 0;
        public int GroupId { get; set; }
        public string? ImagePath { get; set; }
        public TypeEnum Type { get; set; } = TypeEnum.Text;
        public enum TypeEnum
        {
            Image = 2,
            Youtube = 1,
            Text = 0
        }
    }
}
