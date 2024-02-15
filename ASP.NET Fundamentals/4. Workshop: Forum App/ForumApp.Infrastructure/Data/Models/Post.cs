using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static ForumApp.Infrastructure.Constants.ValidationConstants;

namespace ForumApp.Infrastructure.Data.Models
{
    [Comment("Posts table")]
    public class Post
    {
        [Key]
        [Comment("Post Identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(PostTitleMaxLength)]
        [Comment("Post Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(PostContentMaxLength)]
        [Comment("Post Content")]
        public string Content { get; set; } = string.Empty;
    }
}
