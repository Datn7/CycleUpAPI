using AutoMapper;
using CycleUpAPI.Entities;
using CycleUpAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleUpAPI.Controllers
{
    [Route("api/meetup/{meetupName}/hangout")]
    [ApiController]
    public class HangoutsController : ControllerBase
    {
        private readonly CycleContext cycleContext;
        private readonly IMapper mapper;
        private readonly ILogger<HangoutsController> logger;

        public HangoutsController(CycleContext cycleContext, IMapper mapper, ILogger<HangoutsController> logger)
        {
            this.cycleContext = cycleContext;
            this.mapper = mapper;
            this.logger = logger;
        }
        
        [HttpGet]
        public ActionResult Get(string meetupName)
        {
            var meetup = cycleContext.Meetups
               .Include(m => m.Hangouts)
               .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

            if (meetup == null)
            {
                return NotFound();
            }

            var hangouts = mapper.Map<List<HangoutDTO>>(meetup.Hangouts);

            return Ok(hangouts);
        }

        [HttpPost]
        public ActionResult Post(string meetupName, [FromBody]HangoutDTO hangoutDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meetup = cycleContext.Meetups
                .Include(m => m.Hangouts)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

            if (meetup == null)
            {
                return NotFound();
            }

            var hangout = mapper.Map<Hangout>(hangoutDTO);
            meetup.Hangouts.Add(hangout);
            cycleContext.SaveChanges();

            return Created($"api/meetup/{meetupName}",null);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string meetupName, int id)
        {
            var meetup = cycleContext.Meetups
               .Include(m => m.Hangouts)
               .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

            if (meetup == null)
            {
                return NotFound();
            }

            var hangout = meetup.Hangouts.FirstOrDefault(l => l.Id == id);

            if(hangout == null)
            {
                return NotFound();
            }

            cycleContext.Hangouts.Remove(hangout);
            cycleContext.SaveChanges();

            return NoContent();
        }


        [HttpDelete]
        public ActionResult Delete(string meetupName)
        {
            var meetup = cycleContext.Meetups
               .Include(m => m.Hangouts)
               .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

            if (meetup == null)
            {
                return NotFound();
            }

            logger.LogWarning($"Hangouts for meetup {meetupName} were deleted!");

            cycleContext.Hangouts.RemoveRange(meetup.Hangouts);
            cycleContext.SaveChanges();

            return NoContent();
        }
    }
}
