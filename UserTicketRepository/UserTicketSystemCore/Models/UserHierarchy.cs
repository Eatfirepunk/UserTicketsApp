using System;
using System.Collections.Generic;
using System.Text;

namespace UserTicketSystemCore.Models
{
    public class UserHierarchy
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ReportingUserId { get; set; }
        public User ReportingUser { get; set; }
    }
}
