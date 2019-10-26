using AutoMapper;
using Booksy.Dtos;
using Booksy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booksy.Mapper
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
