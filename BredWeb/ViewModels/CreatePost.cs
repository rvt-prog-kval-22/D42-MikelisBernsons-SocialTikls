using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BredWeb.Models
{
    public class CreatePost
    {
        [Required]
        [MaxLength(2000)]
        [MinLength(4)]
        public string? Body { get; set; }
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string? Title { get; set; }
        public DateTime PostDate { get; set; }
        public int GroupId { get; set; }
        public IFormFile? File { get; set; }
        public TypeEnum Type { get; set; } = TypeEnum.Text;
        public enum TypeEnum
        {
            Image = 2,
            Youtube = 1,
            Text = 0
        }
    }
}
