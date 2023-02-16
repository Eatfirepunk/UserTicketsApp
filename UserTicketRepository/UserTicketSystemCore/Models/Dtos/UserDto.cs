using System;
using System.Collections.Generic;
using System.Text;

namespace UserTicketSystemCore.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public ICollection<RoleDto> Roles { get; set; }
        public int? ReportsToId { get; set; } 
    }

}
