﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Helpers;
using UserTicketSystemCore.Interfaces;
using UserTicketSystemCore.Models;
using UserTicketSystemCore.Models.Dtos;

namespace UserTicketSystemData.Repositories
{
    //The repository is not called directly instead a service was created to follow the Open/Closed principle so logic gets added here and not on the upper layers in case you need to extend the functionality
    public class UserRepository : IUserRepository
    {
        private readonly UserTicketSystemContext _context;
        private readonly IMapper _mapper;
        private readonly IUserHierarchyRepository _userHierarchyRepository;

        public UserRepository(UserTicketSystemContext context, IMapper mapper, IUserHierarchyRepository userHierarchyRepository)
        {
            _context = context;
            _mapper = mapper;
            _userHierarchyRepository = userHierarchyRepository;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            try
            {
                var users = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                    .Include(userReportsTo => userReportsTo.ReportedUsers).Include(usr => usr.ReportingUsers)
                    .ToListAsync();
                var mappedUsers = _mapper.Map<IEnumerable<UserDto>>(users);

                int index = 0;
                foreach(var u in mappedUsers) 
                {
                    // get the first user it reports to, this logic can be expanded when the user can report to one or more users, at this time just can report to one
                    var uId = users[index].ReportedUsers?.FirstOrDefault();
                    if (uId != null) 
                    {
                        u.ReportsToId = uId.UserId;
                        u.ReportsToUsername = uId.User.Username;
                    }              
                    index++;
                }

                return mappedUsers;
            }
            catch (Exception ex)
            {
                // Log or handle the error as needed
                throw new Exception("An error occurred while getting users from the database", ex);
            }
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                                    .Include(userReportsTo => userReportsTo.ReportedUsers).Include(usr => usr.ReportingUsers)
                                    .FirstOrDefaultAsync(u => u.Id == id);
                if (user == null) { return null; }
                var mappedUser = _mapper.Map<UserDto>(user);
                // get the first user it reports to, this logic can be expanded when the user can report to one or more users, at this time just can report to one
                var uId = user.ReportedUsers?.FirstOrDefault();
                mappedUser.ReportsToId = uId != null ? uId.UserId : (int?)null;
                mappedUser.ReportsToUsername = uId?.User?.Username;
                return mappedUser;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting user with id {id} from the database", ex);
            }
        }

        public async Task<UserDto> CreateUserAsync(LoginDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                LoginHelpers.AddHashAndSalt(user, userDto);
                user.JwtSecret = LoginHelpers.GenerateJwtSecret();
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating user in the database", ex);
            }
        }

        public async Task UpdateUserAsync(UserDto userDto)
        {
            try
            {
                var user = await _context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == userDto.Id);
                if (user == null)
                {
                    throw new ArgumentException($"User with id {userDto.Id} not found");
                }

                // Update user properties
                _mapper.Map(userDto, user);

                // Update user roles
                user.UserRoles.Clear();
                if (userDto.Roles != null && userDto.Roles.Any())
                {
                    var roleIds = userDto.Roles.Select(r => r.Id);
                    var roles = await _context.Roles.Where(r => roleIds.Contains(r.Id)).ToListAsync();
                    foreach (var role in roles)
                    {
                        user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating user with id {userDto.Id} in the database", ex);
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    throw new ArgumentException($"User with id {id} not found");
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting user with id {id} from the database", ex);
            }
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            //user do not exists or password incorrect return null handle in upper layer
            if (user == null || !LoginHelpers.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                                    .Include(userReportsTo => userReportsTo.ReportedUsers).Include(usr => usr.ReportingUsers)
                                    .FirstOrDefaultAsync(u => u.Email == email);
                if (user == null) { return null; }
                var mappedUser = _mapper.Map<UserDto>(user);
                // get the first user it reports to, this logic can be expanded when the user can report to one or more users, at this time just can report to one
                var uId = user.ReportedUsers?.FirstOrDefault();
                mappedUser.ReportsToId = uId != null ? uId.UserId : (int?)null;
                mappedUser.ReportsToUsername = uId?.User?.Username;
                return mappedUser;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting user with email {email} from the database", ex);
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllManagerSubortinates(int managerId)
        {
            var subordinatesIds = await _userHierarchyRepository.GetUserHierarchyByUserIdAsync(managerId);
            var users = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .Include(userReportsTo => userReportsTo.ReportedUsers).Include(usr => usr.ReportingUsers)
                .Where(x => subordinatesIds.Any(s => s.ReportingUserId == x.Id))
                .ToListAsync();
            var mappedUsers = _mapper.Map<IEnumerable<UserDto>>(users);

            int index = 0;
            foreach (var u in mappedUsers)
            {
                // get the first user it reports to, this logic can be expanded when the user can report to one or more users, at this time just can report to one
                var uId = users[index].ReportedUsers?.FirstOrDefault();
                if (uId != null)
                {
                    u.ReportsToId = uId.UserId;
                    u.ReportsToUsername = uId.User.Username;
                }
                index++;
            }

            return mappedUsers;

        }
    }


}
