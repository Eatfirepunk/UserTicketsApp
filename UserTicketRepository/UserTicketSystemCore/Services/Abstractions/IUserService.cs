using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Models.Dtos;

namespace UserTicketSystemCore.Services.Abstractions
{
   public interface IUserService
    {
        /// <summary>
        /// Method retrieves a user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a UserDto</returns>
        Task<UserDto> GetUserByIdAsync(int id);

        /// <summary>
        /// Method retrieves a user by email
        /// </summary>
        /// <param name="mail"></param>
        /// <returns>Returns a UserDto</returns>
        Task<UserDto> GetUserByEmailAsync(string mail);

        /// <summary>
        /// Method retrieves all the users
        /// </summary>
        /// <returns>IEnumerable<UserDto></returns>
        Task<IEnumerable<UserDto>> GetUsersAsync();

        /// <summary>
        /// method creates a new user
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns>Returns the newly created UserDto</returns>
        Task<UserDto> CreateUserAsync(LoginDto loginDto);

        /// <summary>
        /// Method updates an existing user.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        Task UpdateUserAsync(UserDto userDto);

        /// <summary>
        /// Method deletes a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteUserAsync(int userId);

        /// <summary>
        /// Method authenticates a user 
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns>Returns a JWT token</returns>
        Task<string> LoginAsync(CredentialsDto loginDto);
        /// <summary>
        /// Method retrieves a list of users for drop-down list
        /// </summary>
        /// <returns></returns>
   
        Task<List<DropdownDto>> GetUsersForDropDown();
    }
}
