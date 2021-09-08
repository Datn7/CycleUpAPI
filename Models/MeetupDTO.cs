using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleUpAPI.Models
{
    public class MeetupDTO
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        public string Organizer { get; set; }
        public DateTime Date { get; set; }
        public bool IsPrivate { get; set; }
    }
}
