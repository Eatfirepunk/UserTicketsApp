using System;
using System.Collections.Generic;
using System.Text;
using UserTicketSystemCore.Models;

namespace UserTicketSystemCore.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }

}
