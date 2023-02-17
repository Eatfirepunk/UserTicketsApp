using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserTicketSystemCore.Models;
using UserTicketSystemCore.Models.Dtos;

namespace UsersMicroService.Mappers
{
    public class CustomMapper : Profile
    {
        public CustomMapper()
        {
            CreateMap<UserHierarchy, UserHierarchyDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role)))
                .ReverseMap();
        }
    }
}
