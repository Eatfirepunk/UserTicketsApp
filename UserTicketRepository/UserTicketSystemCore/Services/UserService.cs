using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserTicketSystemCore.Interfaces;
using UserTicketSystemCore.Models.Dtos;
using UserTicketSystemCore.Services.Abstractions;

namespace UserTicketSystemCore
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHierarchyRepository _userHierarchyRepository;

        public UserService(IUserRepository userRepository, IUserHierarchyRepository userHierarchyRepository) 
        {
            _userRepository = userRepository;
            _userHierarchyRepository = userHierarchyRepository;
        }
        public async Task<UserDto> CreateUserAsync(LoginDto loginDto)
        {
            try
            {
                // Check if a user with the provided email already exists
                var existingUser = await _userRepository.GetUserByEmailAsync(loginDto.Email);
                if (existingUser != null)
                {
                    throw new ArgumentException($"A user with email {loginDto.Email} already exists.");
                }

                var newUser = await _userRepository.CreateUserAsync(loginDto);

                if (newUser != null && loginDto.ReportsToId.HasValue)
                {
                    var hierarchy = new UserHierarchyDto { UserId = newUser.Id, ReportingUserId = loginDto.ReportsToId.Value };
                    await _userHierarchyRepository.AddUserHierarchyAsync(hierarchy);
                }

                return await _userRepository.GetUserByIdAsync(newUser.Id);
            }
            catch(Exception ex) 
            {
                throw new Exception($"An error occurred while creating user with email {loginDto.Email}", ex);
            }

        }

        public async Task DeleteUserAsync(int userId)
        {
            try 
            {
                await _userRepository.DeleteUserAsync(userId);
            }
            catch (Exception ex) 
            {
                throw new Exception($"An error occurred while deleting the user {userId}", ex);
            }
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            return await _userRepository.LoginAsync(loginDto);
        }

        public async Task UpdateUserAsync(UserDto userDto)
        {
            try
            {
               await _userRepository.UpdateUserAsync(userDto);
                if (userDto.ReportsToId.HasValue)
                {
                    var hierarchy = new UserHierarchyDto { UserId = userDto.Id, ReportingUserId = userDto.ReportsToId.Value };
                    await _userHierarchyRepository.UpdateUserHierarchyAsync(hierarchy);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the user {userDto.Id}", ex);
            }
        }
    }
}
