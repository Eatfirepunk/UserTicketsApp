using System;
using System.Collections.Generic;
using System.Text;

namespace UserTicketSystemCore.Models.Dtos
{
    public class LoggedUserDto :LoginDto
    {
        public string JwtSecret { get; set; }
        public string JwtToken { get; set; }
    }
}
