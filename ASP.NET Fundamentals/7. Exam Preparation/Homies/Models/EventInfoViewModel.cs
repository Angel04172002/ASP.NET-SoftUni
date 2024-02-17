using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Homies.Data.DataConstants;

namespace Homies.Models
{
    public class EventInfoViewModel
    {
        public EventInfoViewModel(
            int id,
            string name,
            DateTime start,
            string organiser,
            string type)
        {
            Id = id;
            Name = name;
            Start = start.ToString(DateFormat);
            Organiser = organiser;
            Type = type;
        }

        public int Id { get; set; }

        public string Name { get; set; } 

        public string Start { get; set; }

        public string Organiser { get; set; }

        public string Type { get; set; }

    }
}
