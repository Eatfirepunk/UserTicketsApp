using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Interfaces;
using UserTicketSystemCore.Models;
using UserTicketSystemCore.Models.Dtos;

namespace UserTicketSystemData.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserTicketSystemContext _context;
        // mapper not used because the userId comes from a separate parameter not the dto, but will be injected if needed in the future
        //private readonly IMapper _mapper;

        public RoleRepository(UserTicketSystemContext context)
        {
            _context = context;
            //_mapper = mapper;
        }
        public async Task AddUserRoles(ICollection<RoleDto> userRoles,int userId)
        {
            // Remove all existing roles for the user
            _context.UserRoles.RemoveRange(_context.UserRoles.Where(ur => ur.UserId == userId));

            // Add the new roles
            foreach (var roleDto in userRoles)
            {
                var role = await _context.Roles.FindAsync(roleDto.Id);
                if (role == null)
                {
                    throw new ArgumentException($"Role with ID {roleDto.Id} not found.");
                }
                var userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = roleDto.Id,
                    Role = role
                };
                _context.UserRoles.Add(userRole);
            }

            await _context.SaveChangesAsync();
        }
    }
}
