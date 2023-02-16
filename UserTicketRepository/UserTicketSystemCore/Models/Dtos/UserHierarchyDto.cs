using System;
using System.Collections.Generic;
using System.Text;

namespace UserTicketSystemCore.Models.Dtos
{
    public class UserHierarchyDto
    {
        public int UserId { get; set; }
        public int ReportingUserId { get; set; }
    }

}
