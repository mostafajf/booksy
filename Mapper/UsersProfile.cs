using AutoMapper;
using Beauty.Dtos;
using Beauty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beauty.Mapper
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<ApplicationUser, UserDto>().ForPath(dest => dest.Location.Latitude, opt =>
            {
                opt.MapFrom(src => src.Location.Coordinates.Latitude);
            }).ForPath(dest => dest.Location.Longitude, opt =>
            {
                opt.MapFrom(src => src.Location.Coordinates.Longitude);
            });
        }
    }
}
