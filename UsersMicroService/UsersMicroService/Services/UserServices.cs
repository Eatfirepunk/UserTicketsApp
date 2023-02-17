using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Interfaces;
using UserTicketSystemCore.Models.Dtos;
using UserTicketSystemCore.Services.Abstractions;

namespace UsersMicroService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHierarchyRepository _userHierarchyRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _configuration;
        public UserService(IUserRepository userRepository, 
            IUserHierarchyRepository userHierarchyRepository
            ,IRoleRepository roleRepository,IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userHierarchyRepository = userHierarchyRepository;
            _roleRepository = roleRepository;
            _configuration = configuration;
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

                bool isDeveloperTester = loginDto.ReportsToId.HasValue && loginDto.ReportsToId.Value != 0;
                var newUser = await _userRepository.CreateUserAsync(loginDto);

                if (newUser != null && isDeveloperTester)
                {
                    var hierarchy = new UserHierarchyDto { UserId = loginDto.ReportsToId.Value, ReportingUserId = newUser.Id  };
                    await _userHierarchyRepository.AddUserHierarchyAsync(hierarchy);
                }


                await AddUpdateUserRoles(loginDto, newUser.Id, isDeveloperTester);

                return await _userRepository.GetUserByIdAsync(newUser.Id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating user with email {loginDto.Email}", ex);
            }

        }

        private async Task AddUpdateUserRoles(UserDto userDto,int id,bool isDeveloperTester) 
        {
            userDto.Roles = userDto.Roles ?? new List<RoleDto>();
            RoleDto defaultRole = new RoleDto();

            // if the user reports to someone then is a developer/tester if not then is a lead/manager
            // TODO change hardcoded roleIds for repository lookups

            if (isDeveloperTester)
            {
                //DEveloper/tester role
                defaultRole.Id = 2;
            }
            else
            {
                //Lead/Manager role
                defaultRole.Id = 3;
            }

            userDto.Roles.Add(defaultRole);

            await _roleRepository.AddUserRoles(userDto.Roles, id);
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

        public async Task<string> LoginAsync(LoginDto loginDto)
        {

            // Check if the user credentials are valid
            var loginValid = await _userRepository.LoginAsync(loginDto);
            if (loginValid == null)
            {
                return "Unauthorized";
            }

            List<Claim> claims = new List<Claim>();
            foreach(var rol in loginValid.Roles) 
            {
                var claim = new Claim(ClaimTypes.Role,rol.Name);
                claims.Add(claim);
            }
            claims.Add(new Claim(ClaimTypes.Name, loginValid.Username));
            // Generate a JWT token
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(7),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public async Task UpdateUserAsync(UserDto userDto)
        {
            try
            {
                bool isDeveloperTester = userDto.ReportsToId.HasValue && userDto.ReportsToId.Value != 0;
                await _userRepository.UpdateUserAsync(userDto);
                if (userDto.ReportsToId.HasValue)
                {
                    var hierarchy = new UserHierarchyDto { UserId = userDto.ReportsToId.Value , ReportingUserId = userDto.Id };
                    await _userHierarchyRepository.UpdateUserHierarchyAsync(hierarchy);
                }

                await AddUpdateUserRoles(userDto, userDto.Id, isDeveloperTester);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the user {userDto.Id}", ex);
            }
        }
    }
}
