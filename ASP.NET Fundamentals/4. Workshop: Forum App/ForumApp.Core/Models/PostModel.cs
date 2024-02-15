using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static ForumApp.Infrastructure.Constants.ValidationConstants;


namespace ForumApp.Core.Models
{
    /// <summary>
    /// Post model
    /// </summary>
    public class PostModel
    {
        /// <summary>
        /// Post Identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Post Title
        /// </summary>

        [Required(ErrorMessage = RequireErrorMessage)]
        [StringLength(PostTitleMaxLength, MinimumLength = PostTitleMinLength, ErrorMessage = IncorrectLengthErrorMessage)]
        public string Title { get; set; } = string.Empty;


        /// <summary>
        /// Post Content
        /// </summary>

        [Required(ErrorMessage = RequireErrorMessage)]
        [StringLength(PostContentMaxLength, MinimumLength = PostContentMinLength, ErrorMessage = IncorrectLengthErrorMessage)]
        public string Content { get; set; } = string.Empty;
    }
}
