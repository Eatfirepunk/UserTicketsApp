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

        /// <summary>
        /// Retrieves a list of all users from the database, including their roles and reporting relationships.
        /// </summary>
        /// <returns>Returns an enumerable collection of UserDto objects</returns>
        Task<IEnumerable<UserDto>> GetUsersAsync();
        /// <summary>
        /// Retrieves a single user by their ID from the database, including their roles and reporting relationships
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a UserDto object, or null if the user does not exist.</returns>
        Task<UserDto> GetUserByIdAsync(int id);

        /// <summary>
        /// Creates a new user in the database based on a LoginDto object, which contains the user's email, password, and roles.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns> Returns a UserDto object representing the newly created user</returns>
        Task<UserDto> CreateUserAsync(LoginDto userDto);

        /// <summary>
        /// Updates an existing user in the database based on a UserDto object, including updating the user's roles. 
        /// </summary>
        /// <param name="userDto"></param>
        /// <exception cref="ArgumentException">Throws an ArgumentException if the user cannot be found.</exception>
        /// <returns></returns>
        Task UpdateUserAsync(UserDto user);

        /// <summary>
        /// Deletes an existing user from the database by their ID
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ArgumentException">Throws an ArgumentException if the user cannot be found.</exception>
        /// <returns></returns>
        Task DeleteUserAsync(int id);
        /// <summary>
        /// Authenticates a user based on their email and password by verifying the hashed password stored in the database.
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns>Returns a UserDto object representing the authenticated user, or null if authentication fails</returns>
        Task<UserDto> LoginAsync(CredentialsDto loginDto);
        /// <summary>
        /// Retrieves a single user by their email address from the database, including their roles and reporting relationships
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Returns a UserDto object, or null if the user does not exist.</returns>
        Task<UserDto> GetUserByEmailAsync(string email);
        /// <summary>
        /// Retrieves a list of all users who report to the specified manager, including their roles and reporting relationships. 
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns>Returns an enumerable collection of UserDto objects</returns>
        Task<IEnumerable<UserDto>> GetAllManagerSubortinates(int managerId);
    }

}
