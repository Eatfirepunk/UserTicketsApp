using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class UserHierarchyRepository : IUserHierarchyRepository
    {
        private readonly UserTicketSystemContext _context;
        private readonly IMapper _mapper;

        public UserHierarchyRepository(UserTicketSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserHierarchyDto> GetUserHierarchyByIdAsync(int id)
        {
            var userHierarchy = await _context.UserHierarchies.FindAsync(id);

            if (userHierarchy == null)
            {
                throw new ArgumentException($"User hierarchy with id {id} not found.");
            }

            return _mapper.Map<UserHierarchyDto>(userHierarchy);
        }

        public async Task<IEnumerable<UserHierarchyDto>> GetUserHierarchyByUserIdAsync(int userId)
        {
            var userHierarchies = await _context.UserHierarchies
                .Where(uh => uh.UserId == userId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserHierarchyDto>>(userHierarchies);
        }

        public async Task<IEnumerable<UserHierarchyDto>> GetUserHierarchyByReportingUserIdAsync(int reportingUserId)
        {
            var userHierarchies = await _context.UserHierarchies
                .Where(uh => uh.ReportingUserId == reportingUserId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserHierarchyDto>>(userHierarchies);
        }

        public async Task AddUserHierarchyAsync(UserHierarchyDto userHierarchyDto)
        {
            var userHierarchy = _mapper.Map<UserHierarchy>(userHierarchyDto);

            _context.UserHierarchies.Add(userHierarchy);
            await _context.SaveChangesAsync();

            userHierarchyDto.Id = userHierarchy.Id;
        }

        public async Task UpdateUserHierarchyAsync(UserHierarchyDto userHierarchyDto)
        {
            var userHierarchy = await _context.UserHierarchies.FindAsync(userHierarchyDto.Id);

            if (userHierarchy == null)
            {
                throw new ArgumentException($"User hierarchy with id {userHierarchyDto.Id} not found.");
            }

            _mapper.Map(userHierarchyDto, userHierarchy);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserHierarchyAsync(int userId)
        {
            var userHierarchiesToRemove = await _context.UserHierarchies
                .Where(uh => uh.ReportingUserId == userId)
                .ToListAsync();

            _context.UserHierarchies.RemoveRange(userHierarchiesToRemove);

            await _context.SaveChangesAsync();
        }
    }


}
