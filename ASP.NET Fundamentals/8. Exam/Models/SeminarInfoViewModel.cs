using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SeminarHub.Data;

namespace SeminarHub.Models
{
    public class SeminarInfoViewModel
    {
        public SeminarInfoViewModel(
            int id,
            string topic,
            string lecturer,
            string category,
            string dateAndTime,
            string organizer
            )
        {
            Id = id;
            Topic = topic;
            Lecturer = lecturer;
            Category = category;
            DateAndTime = dateAndTime;
            Organizer = organizer;
        }


        /// <summary>
        /// Seminar Identifier
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Seminar Topic
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Seminar Lecturer
        /// </summary>
        public string Lecturer { get; set; }

        /// <summary>
        /// Seminar Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Seminar Date
        /// </summary>
        public string DateAndTime { get; set; }

        /// <summary>
        /// Seminar Organizer
        /// </summary>
        public string Organizer { get; set; }

    }
}
