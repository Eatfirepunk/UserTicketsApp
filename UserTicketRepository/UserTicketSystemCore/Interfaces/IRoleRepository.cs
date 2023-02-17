using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Models.Dtos;

namespace UserTicketSystemCore.Interfaces
{
    public interface IRoleRepository
    {
        public Task AddUserRoles(ICollection<RoleDto> userRoles,int userId);
    }
}
