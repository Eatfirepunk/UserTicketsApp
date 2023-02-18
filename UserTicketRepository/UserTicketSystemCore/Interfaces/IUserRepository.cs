using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Models;
using UserTicketSystemCore.Models.Dtos;

namespace UserTicketSystemCore.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(LoginDto userDto);
        Task UpdateUserAsync(UserDto user);
        Task DeleteUserAsync(int id);

        Task<UserDto> LoginAsync(LoginDto loginDto);

        Task<UserDto> GetUserByEmailAsync(string email);

    }

}
