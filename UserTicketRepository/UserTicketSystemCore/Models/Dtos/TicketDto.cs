using System;
using System.Collections.Generic;
using System.Text;

namespace UserTicketSystemCore.Models.Dtos
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TicketType { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }
}
