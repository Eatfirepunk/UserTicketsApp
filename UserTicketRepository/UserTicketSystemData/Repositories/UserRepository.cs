using AutoMapper;
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
    public class UserRepository : IUserRepository
    {
        private readonly UserTicketSystemContext _context;
        private readonly IMapper _mapper;

        public UserRepository(UserTicketSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            try
            {
                var users = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToListAsync();
                return _mapper.Map<IEnumerable<UserDto>>(users);
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
                var user = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.Id == id);
                return _mapper.Map<UserDto>(user);
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
                var ExistingUser = await _context.Users.AnyAsync(u => u.Email == userDto.Email);
                if (ExistingUser)
                {
                    throw new ArgumentException($"Duplicate user email");
                }
                var user = _mapper.Map<User>(userDto);
                LoginHelpers.AddHashAndSalt(user, userDto);
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                throw new ArgumentException($"User with email {loginDto.Email} not found");
            }

            if (!LoginHelpers.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new ArgumentException("Invalid password");
            }

            return _mapper.Map<UserDto>(user);
        }
    }


}
