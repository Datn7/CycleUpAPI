using AutoMapper;
using CycleUpAPI.Controllers.Filters;
using CycleUpAPI.Entities;
using CycleUpAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleUpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TimeTrackFilter]
    public class RMeetupController : ControllerBase
    {
        private readonly CycleContext cycleContext;
        private readonly IMapper mapper;

        public RMeetupController(CycleContext cycleContext, IMapper mapper)
        {
            this.cycleContext = cycleContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<MeetupDetailsDTO>> Get()
        {
            //locate meetups and include location aswell because in automapper we map them.
            var meetups = cycleContext.Meetups.Include(m=>m.Location).ToList();

            //map it, choose target class and (source class)
            var meetupDTOs = mapper.Map<List<MeetupDetailsDTO>>(meetups);

            return Ok(meetupDTOs);
        }

        [HttpGet("{name}")]
        public ActionResult<MeetupDetailsDTO> Get(string name)
        {
            var meetup = cycleContext.Meetups
                .Include(m => m.Location) 
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

            if(meetup == null)
            {
                return NotFound();
            }

            var meetupDto = mapper.Map<MeetupDetailsDTO>(meetup);

            return Ok(meetupDto);
        }

        [HttpPost]
        public ActionResult Post([FromBody]MeetupDTO meetupDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meetup = mapper.Map<Meetup>(meetupDTO);
            cycleContext.Meetups.Add(meetup);
            cycleContext.SaveChanges();

            var key = meetup.Name.Replace(" ", "-").ToLower();

            return Created("api/meetup/" + key, null);
            
        }

        [HttpPut("{name}")]
        public ActionResult Put(string name, [FromBody] MeetupDTO meetupDTO)
        {
            var meetup = cycleContext.Meetups
              .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

            if (meetup == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            meetup.Name = meetupDTO.Name;
            meetup.Organizer = meetupDTO.Organizer;
            meetup.Date = meetupDTO.Date;
            meetup.IsPrivate = meetupDTO.IsPrivate;

            cycleContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{name}")]
        public ActionResult Delete(string name)
        {
            var meetup = cycleContext.Meetups
              .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

            if (meetup == null)
            {
                return NotFound();
            }

            cycleContext.Remove(meetup);
            cycleContext.SaveChanges();

            return NoContent();
        }
    }
}
