namespace SeminarHub.Models
{
    public class DeleteFormViewModel
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
        public DateTime DateAndTime { get; set; }
    }
}
