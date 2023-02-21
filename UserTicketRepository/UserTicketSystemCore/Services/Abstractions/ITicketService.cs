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

        /// <summary>
        /// returns all tickets based on the given TicketLookUpParameters filters.
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        Task<IEnumerable<TicketDto>> GetAllTicketsAsync(TicketLookUpParameters filters);

        /// <summary>
        /// returns all tickets assigned to the given lead manager user and their subordinates based on the given TicketLookUpParameters filters.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<TicketDto>> GetAllTicketsUnderLeadManagerAsync(TicketLookUpParameters filters, int userId);

        /// <summary>
        /// returns all tickets assigned to the given user based on the given TicketLookUpParameters filters
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<TicketDto>> GetAllTicketsForUserAsync(TicketLookUpParameters filters, int userId);

        /// <summary>
        ///  returns a ticket with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> returns a ticket with the given id</returns>
        Task<TicketDto> GetTicketByIdAsync(Guid id);

        /// <summary>
        /// creates a new ticket with the information in the given TicketDto 
        /// </summary>
        /// <param name="ticketDto"></param>
        /// <returns>the created ticket</returns>
        Task<TicketDto> CreateTicketAsync(TicketDto ticketDto);

        /// <summary>
        /// updates a ticket with the information in the given TicketDto
        /// </summary>
        /// <param name="ticketDto"></param>
        /// <returns></returns>
        Task UpdateTicketAsync(TicketDto ticketDto);

        /// <summary>
        /// deletes a ticket with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteTicketAsync(Guid id);
    }
}
