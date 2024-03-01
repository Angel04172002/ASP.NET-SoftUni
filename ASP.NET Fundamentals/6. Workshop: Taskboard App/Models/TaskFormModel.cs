using System.ComponentModel.DataAnnotations;
using static TaskBoardApp.Data.DataConstants;

namespace TaskBoardApp.Models
{
    public class TaskFormModel
    {
        [Required(ErrorMessage = "Field {0} is required!")]
        [StringLength(
            TaskTitleMaxLength,
            MinimumLength = TaskTitleMinLength,
            ErrorMessage = "Field {0} must be between {2} and {1} characters long!")]
        public string Title { get; set; } = string.Empty;


        [Required(ErrorMessage = "Field {0} is required!")]
        [StringLength(
            TaskDescriptionMaxLength,
            MinimumLength = TaskDescriptionMinLength,
            ErrorMessage = "Field {0} must be between {2} and {1} characters long!")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Board")]
        public int BoardId { get; set; }

        public IEnumerable<BoardSelectViewModel>? Boards { get; set; }
    }
}
