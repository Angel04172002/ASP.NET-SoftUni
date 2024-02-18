using System.ComponentModel.DataAnnotations;
using static SeminarHub.Data.DataConstants;
using static SeminarHub.Models.ErrorConstants;


namespace SeminarHub.Models
{
    public class SeminarFormViewModel
    {
        /// <summary>
        /// Seminar Topic
        /// </summary>

        [Required(ErrorMessage = RequireErrorMessage)]
        [StringLength(
            SeminarTopicMaxLength,
            MinimumLength = SeminarTopicMinLength,
            ErrorMessage = StringLengthErrorMessage)]
        public string Topic { get; set; } = string.Empty;

        /// <summary>
        /// Seminar Lecturer
        /// </summary>

        [Required(ErrorMessage = RequireErrorMessage)]
        [StringLength(
            SeminarLecturerMaxLength,
            MinimumLength = SeminarLecturerMinLength,
            ErrorMessage = StringLengthErrorMessage)]
           
        public string Lecturer { get; set; } = string.Empty;

        /// <summary>
        /// Seminar Details
        /// </summary>

        [Required(ErrorMessage = RequireErrorMessage)]
        [StringLength(
            SeminarDetailsMaxLength,
            MinimumLength = SeminarDetailsMinLength,
            ErrorMessage = StringLengthErrorMessage)]
        public string Details { get; set; } = string.Empty;


        /// <summary>
        /// Seminar Date
        /// </summary>

        [Required(ErrorMessage = RequireErrorMessage)]
        public string DateAndTime { get; set; } = string.Empty;

        /// <summary>
        /// Seminar Duration
        /// </summary>

        [Range(
            SeminarMinDuration, 
            SeminarMaxDuration, 
            ErrorMessage = IncorrectDurationErrorMessage)]
        public int? Duration { get; set; }


        /// <summary>
        /// Seminar Category Id
        /// </summary>

        [Required(ErrorMessage = RequireErrorMessage)]
        public int CategoryId { get; set; }

        /// <summary>
        /// Seminar Categories
        /// </summary>
        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
