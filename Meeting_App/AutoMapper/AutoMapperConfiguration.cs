using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Meeting_App.ORM;
using Meeting_App.Models;

namespace Meeting_App.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            //Mapper.CreateMap<Attendee, tbl_Attendee>()
            //     .ForMember(Attendee => Attendee. , opt => opt.Ignore())
            //     .ForMember(dest => dest.FoundingDate, opt => opt.MapFrom(src => src.OrganizationDate));
            //var config = new MapperConfiguration(m => m.CreateMap<tbl_Attendee, Attendee>());
            //var mapper = config.CreateMapper();
            //var model = mapper.Map<Attendee>(tbl_Attendee);
            
        }
    }
}