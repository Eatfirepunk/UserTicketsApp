using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Models.Dtos;
using UserTicketSystemCore.Models.LookUpModels;

namespace UserTicketSystemCore.Services.Abstractions
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>> GetAllTicketsAsync(TicketLookUpParameters filters);
        Task<IEnumerable<TicketDto>> GetAllTicketsUnderLeadManagerAsync(TicketLookUpParameters filters, int userId);
        Task<IEnumerable<TicketDto>> GetAllTicketsForUserAsync(TicketLookUpParameters filters, int userId);
        Task<TicketDto> GetTicketByIdAsync(Guid id);
        Task<TicketDto> CreateTicketAsync(TicketDto ticketDto);
        Task UpdateTicketAsync(TicketDto ticketDto);
        Task DeleteTicketAsync(Guid id);
    }
}
