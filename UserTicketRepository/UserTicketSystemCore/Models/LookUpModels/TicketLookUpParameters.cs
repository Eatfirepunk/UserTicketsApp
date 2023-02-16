using System;
using System.Collections.Generic;
using System.Text;

namespace UserTicketSystemCore.Models.LookUpModels
{
    public class TicketLookUpParameters
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int? TicketType { get; set; }

        public string TicketTitle { get; set; }
        public string Description { get; set; }
    }
}
