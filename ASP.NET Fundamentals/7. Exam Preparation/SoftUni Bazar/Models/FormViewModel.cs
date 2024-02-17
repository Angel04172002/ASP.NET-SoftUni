using System.ComponentModel.DataAnnotations;
using static SoftUniBazar.Data.DataConstants;

namespace SoftUniBazar.Models
{
    public class FormViewModel
    {
        [Required(ErrorMessage = RequireErrorMessage)]
        [StringLength(
            AdNameMaxLength,
            MinimumLength = AdNameMinLength,
            ErrorMessage = StringLengthErrorMessage
            )]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = RequireErrorMessage)]
        [StringLength(
           AdDescriptionMaxLength,
           MinimumLength = AdDescriptionMinLength,
           ErrorMessage = StringLengthErrorMessage
           )]
        public string Description { get; set; } = string.Empty;


        [Required(ErrorMessage = RequireErrorMessage)]
        public string ImageUrl { get; set; } = string.Empty;


        [Required(ErrorMessage = RequireErrorMessage)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = RequireErrorMessage)]
        public int CategoryId { get; set; }

        public IList<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
