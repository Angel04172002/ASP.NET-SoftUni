using System.ComponentModel.DataAnnotations;

namespace Text_Splitter.Models
{
    public class TextViewModel
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Text { get; set; } = string.Empty;

        public string SplitText { get; set; } = string.Empty;


    }
}
