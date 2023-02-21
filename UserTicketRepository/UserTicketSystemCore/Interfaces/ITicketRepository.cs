using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Models.Dtos;
using UserTicketSystemCore.Models.LookUpModels;

namespace UserTicketSystemCore.Interfaces
{
    public interface ITicketRepository
    {
        /// <summary>
        /// Returns all the tickets stored in the database according to the filters given as input, and maps them to TicketDto objects.
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        Task<IEnumerable<TicketDto>> GetAllTicketsAsync(TicketLookUpParameters filters);
        /// <summary>
        /// Returns all tickets assigned to a specific user, and maps them to TicketDto objects.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<TicketDto>> GetAllTicketsForUserAsync(TicketLookUpParameters filters, int userId);
        /// <summary>
        /// Returns a TicketDto object for a specific ticket identified by the id given as input.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TicketDto> GetTicketByIdAsync(Guid id);
        /// <summary>
        /// Creates a new ticket in the database based on a TicketDto object given as input, and returns the new ticket mapped to a TicketDto object.
        /// </summary>
        /// <param name="ticketDto"></param>
        /// <returns></returns>
        Task<TicketDto> CreateTicketAsync(TicketDto ticketDto);
        /// <summary>
        /// Updates a ticket in the database based on a TicketDto object given as input, and saves the changes.
        /// </summary>
        /// <param name="ticketDto"></param>
        /// <returns></returns>
        Task UpdateTicketAsync(TicketDto ticketDto);
        /// <summary>
        /// Deletes a ticket from the database based on the ticket id given as input, and saves the changes.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteTicketAsync(Guid id);
        /// <summary>
        /// Returns all tickets assigned to a manager and their subordinates, according to filters given as input, and maps them to TicketDto objects.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="managerId"></param>
        /// <returns></returns>
        Task<IEnumerable<TicketDto>> GetAllTicketsUnderManagerAsync(TicketLookUpParameters filters, int managerId);
    }
}
