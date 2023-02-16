using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Models.Dtos;

namespace UserTicketSystemCore.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<TicketDto>> GetAllTicketsAsync();
        Task<TicketDto> GetTicketByIdAsync(Guid id);
        Task<TicketDto> CreateTicketAsync(TicketDto ticketDto);
        Task UpdateTicketAsync(TicketDto ticketDto);
        Task DeleteTicketAsync(Guid id);
    }
}
