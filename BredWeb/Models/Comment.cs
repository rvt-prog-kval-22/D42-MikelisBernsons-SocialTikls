using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(500)]
        public string Body { get; set; }
        public DateTime PostDate { get; set; }
        public bool IsEdited { get; set; }
        public string AuthorName { get; set; }
        public List<Rating> RatingList { get; set; }
        public int Rating { get; set; }
    }
}
