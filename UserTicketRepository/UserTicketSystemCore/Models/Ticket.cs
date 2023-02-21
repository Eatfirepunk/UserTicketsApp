using System;
using System.Collections.Generic;
using System.Text;

namespace UserTicketSystemCore.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public Guid TicketId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TicketTypeId { get; set; }
        public TicketType TicketType { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public int StatusId { get; set; }
        public TicketStatus TicketStatus { get; set; }

        public int? AssignedToId{ get; set; }
        public User AssignedToUser { get; set; }
        public int CreatedBy { get; set; }
        public User CreatedByUser { get; set; }
        public int? UpdatedBy { get; set; }
        public User UpdatedByUser { get; set; }
    }
}
