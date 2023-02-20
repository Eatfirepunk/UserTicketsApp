using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UserTicketSystemCore.Models.Dtos;
using UserTicketSystemCore.Services.Abstractions;

namespace UsersMicroService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        public UsersController(ILogger<UsersController> logger,IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrLeadManager")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody]LoginDto loginModel)
        {

            var tokenString =  _userService.LoginAsync(loginModel).Result;

            if (tokenString != "Unauthorized")
            {

                return Ok(new { Token = tokenString });
            }
            else 
            {
                return Unauthorized("User do not exists or Invalid Credentials");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if(user == null) 
            {
                return NotFound("User do not exists");
            }
            return Ok(user);
        }


        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User do not exists");
            }
            return Ok(user);
        }

        [HttpGet("Catalog")]
        public async Task<IActionResult> GetAllUsersForDropdown()
        {
            var users = await _userService.GetUsersForDropDown();
            return Ok(users);
        }


        [HttpPost]
        [Authorize(Policy = "AdminOrLeadManager")]
        public async Task<IActionResult> CreateUser(LoginDto userDto)
        {
            await _userService.CreateUserAsync(userDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            // if user is not a manager , admin or lead then it cannot modify other user's profile
            if (id != userDto.Id && !userDto.Roles.Any(x=> x.Id == 1 || x.Id == 3))
            {
                return BadRequest("User ID in the request body does not match the ID in the URL.");
            }

            try
            {
                await _userService.UpdateUserAsync(userDto);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOrLeadManager")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
