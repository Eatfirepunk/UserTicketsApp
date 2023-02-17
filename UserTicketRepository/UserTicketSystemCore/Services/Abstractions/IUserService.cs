using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Models.Dtos;

namespace UserTicketSystemCore.Services.Abstractions
{
   public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> CreateUserAsync(LoginDto loginDto);
        Task UpdateUserAsync(UserDto userDto);
        Task DeleteUserAsync(int userId);
        Task<string> LoginAsync(LoginDto loginDto);
    }
}
