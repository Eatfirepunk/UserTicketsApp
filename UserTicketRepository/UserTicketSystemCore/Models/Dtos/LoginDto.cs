using System;
using System.Collections.Generic;
using System.Text;

namespace UserTicketSystemCore.Models.Dtos
{
    public class LoginDto : UserDto
    {
        public string Password { get; set; }
    }
}
