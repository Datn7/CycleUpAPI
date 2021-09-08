using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleUpAPI.Entities
{
    public class Hangout
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public string Topic { get; set; }
        public int MeetupId { get; set; }

        public virtual Meetup Meetup { get; set; }

    }
}
