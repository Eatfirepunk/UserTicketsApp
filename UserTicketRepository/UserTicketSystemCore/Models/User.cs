using System;
using System.Collections.Generic;
using System.Text;

namespace UserTicketSystemCore.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string JwtSecret { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Who reports to this user
        /// </summary>
        public ICollection<UserHierarchy> ReportingUsers { get; set; }

        /// <summary>
        /// Who this user reports to
        /// </summary>
        public ICollection<UserHierarchy> ReportedUsers { get; set; }
        public ICollection<Ticket> CreatedTickets { get; set; }
        public ICollection<Ticket> UpdatedTickets { get; set; }

        public ICollection<Ticket> AssignedTickets { get; set; }
    }

}
