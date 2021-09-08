using AutoMapper;
using CycleUpAPI.Entities;
using CycleUpAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleUpAPI.Automapper
{
    public class MeetupProfile : Profile
    {
        public MeetupProfile()
        {
            CreateMap<Meetup, MeetupDetailsDTO>()
                .ForMember(d => d.City, s => s.MapFrom(m => m.Location.City))
                .ForMember(d => d.Street, s => s.MapFrom(m => m.Location.Street));

            CreateMap<MeetupDTO, Meetup>();


            //may have to reverse those
            CreateMap<HangoutDTO, Hangout>().ReverseMap();
        }
    }
}
