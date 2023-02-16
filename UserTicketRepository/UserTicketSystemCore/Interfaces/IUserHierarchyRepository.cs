using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Models.Dtos;

namespace UserTicketSystemCore.Interfaces
{
    public interface IUserHierarchyRepository
    {
        Task<UserHierarchyDto> GetUserHierarchyByIdAsync(int id);
        Task<IEnumerable<UserHierarchyDto>> GetUserHierarchyByUserIdAsync(int userId);
        Task<IEnumerable<UserHierarchyDto>> GetUserHierarchyByReportingUserIdAsync(int reportingUserId);
        Task AddUserHierarchyAsync(UserHierarchyDto userHierarchyDto);
        Task UpdateUserHierarchyAsync(UserHierarchyDto userHierarchyDto);
        Task DeleteUserHierarchyAsync(int userId);
    }

}
