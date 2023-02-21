using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserTicketSystemCore.Models;
using UserTicketSystemCore.Models.Dtos;

namespace RepositoryTests.TestMappings
{
    public class CustomMapping : Profile
    {
        public CustomMapping()
        {
            CreateMap<UserHierarchy, UserHierarchyDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role)))
                .ReverseMap();
            CreateMap<Ticket, TicketDto>().ReverseMap();
            CreateMap<int, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));
            CreateMap<Ticket, TicketDto>()
                .ForMember(dest => dest.TicketType, opt => opt.MapFrom(src => src.TicketType.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TicketStatus.Name))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedByUser))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedByUser))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedToUser));
        }
    }
}

