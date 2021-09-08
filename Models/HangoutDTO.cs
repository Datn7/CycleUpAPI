using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleUpAPI.Models
{
    public class HangoutDTO
    {
        [Required]
        [MinLength(5)]
        public string Host { get; set; }
        [Required]
        [MinLength(5)]
        public string Topic { get; set; }
    }
}
