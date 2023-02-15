using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserTicketSystemCore.Interfaces;
using UserTicketSystemCore.Models;

namespace UserTicketSystemData.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserTicketSystemContext _context;

        public UserRepository(UserTicketSystemContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                return _context.Users.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all users.", ex);
            }
        }

        public User GetUserById(int id)
        {
            try
            {
                return _context.Users.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user with ID {id}.", ex);
            }
        }

        public void CreateUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating user.", ex);
            }
        }

        public void UpdateUser(User user)
        {
            try
            {
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user with ID {user.Id}.", ex);
            }
        }

        public void DeleteUser(User user)
        {
            try
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting user with ID {user.Id}.", ex);
            }
        }
    }

}
