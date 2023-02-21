using System;
using System.Collections.Generic;
using System.Text;

namespace UserTicketSystemCore.Models.Dtos
{
    public class TicketDto
    {
        public int Id { get; set; }
        public Guid TicketId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TicketTypeId { get; set; }
        public string TicketType { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public UserDto CreatedBy { get; set; }
        public UserDto UpdatedBy { get; set; }
        public UserDto AssignedTo { get; set; }
    }
}
