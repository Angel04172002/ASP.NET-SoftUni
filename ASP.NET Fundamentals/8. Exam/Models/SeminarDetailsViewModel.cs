namespace SeminarHub.Models
{
    public class SeminarDetailsViewModel
    {
        /// <summary>
        /// Seminar Identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Seminar Topic
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Seminar Date
        /// </summary>
        public string DateAndTime { get; set; }

        /// <summary>
        /// Seminar Duration
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        /// Seminar Lecturer
        /// </summary>
        public string Lecturer { get; set; }

        /// <summary>
        /// Seminar Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Seminar Details
        /// </summary>
        public string Details { get; set; } = string.Empty;

        /// <summary>
        /// Seminar Organizer
        /// </summary>
        public string Organizer { get; set; }
    }
}
