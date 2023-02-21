using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Interfaces;
using UserTicketSystemCore.Models;
using UserTicketSystemCore.Models.Dtos;
using UserTicketSystemCore.Models.LookUpModels;

namespace UserTicketSystemData.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly UserTicketSystemContext _context;
        private readonly IMapper _mapper;
        private IUserRepository _userRepository;
        public TicketRepository(UserTicketSystemContext context, IMapper mapper,IUserRepository userRepository)
        {
            _context = context;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Returns all the tickets stored in the database according to the filters given as input, and maps them to TicketDto objects.
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync(TicketLookUpParameters filters)
        {
            var ticketsQuery = _context.Tickets.AsQueryable();
            
            var tickets = await BuildTicketFilters(filters, ticketsQuery);

            return _mapper.Map<IEnumerable<TicketDto>>(tickets);
        }

        // separated method in case retrieving only one user tickets has additional logic
        /// <summary>
        /// Returns all tickets assigned to a specific user, and maps them to TicketDto objects.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TicketDto>> GetAllTicketsForUserAsync(TicketLookUpParameters filters, int userId)
        {
            var ticketsQuery = _context.Tickets.Where(x=> x.AssignedToId == userId);
            var tickets = await BuildTicketFilters(filters, ticketsQuery);

            return _mapper.Map<IEnumerable<TicketDto>>(tickets);
        }


        /// <summary>
        /// Returns all tickets assigned to a manager and their subordinates, according to filters given as input, and maps them to TicketDto objects.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="managerId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TicketDto>> GetAllTicketsUnderManagerAsync(TicketLookUpParameters filters, int managerId)
        {
            var usersUnderManager = await _userRepository.GetAllManagerSubortinates(managerId);
            var userIds = usersUnderManager.Select(x => x.Id).Distinct().ToList();
            var ticketsQuery = _context.Tickets.Where(x => (x.AssignedToId.HasValue && userIds.Contains(x.AssignedToId.Value)) || x.AssignedToId == managerId);

            var tickets = await BuildTicketFilters(filters, ticketsQuery);

            return _mapper.Map<IEnumerable<TicketDto>>(tickets);
        }

        /// <summary>
        /// Private method that applies filtering to a given IQueryable of Ticket objects, according to the filters given as input.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="ticketsQuery"></param>
        /// <returns></returns>
        private async Task<IEnumerable<Ticket>> BuildTicketFilters(TicketLookUpParameters filters, IQueryable<Ticket> ticketsQuery) 
        {
            if (filters.FromDate.HasValue)
            {
                ticketsQuery = ticketsQuery.Where(t => t.CreatedDatetime >= filters.FromDate.Value);
            }

            if (filters.ToDate.HasValue)
            {
                ticketsQuery = ticketsQuery.Where(t => t.CreatedDatetime <= filters.ToDate.Value);
            }

            if (filters.TicketType.HasValue)
            {
                ticketsQuery = ticketsQuery.Where(t => t.TicketType.Id == filters.TicketType.Value);
            }

            if (!string.IsNullOrEmpty(filters.TicketTitle))
            {
                ticketsQuery = ticketsQuery.Where(t => t.Title.Contains(filters.TicketTitle));
            }

            if (!string.IsNullOrEmpty(filters.Description))
            {
                ticketsQuery = ticketsQuery.Where(t => t.Description.Contains(filters.Description));
            }

            if (filters.UpdatedBy.HasValue)
            {
                ticketsQuery = ticketsQuery.Where(t => t.UpdatedBy == filters.UpdatedBy.Value);
            }

            if (filters.CreatedBy.HasValue)
            {
                ticketsQuery = ticketsQuery.Where(t => t.CreatedBy == filters.CreatedBy.Value);
            }

            var tickets = await ticketsQuery
                .Include(t => t.TicketType)
                .Include(t => t.TicketStatus)
                .Include(t => t.CreatedByUser)
                .Include(t => t.UpdatedByUser)
                .Include(t => t.AssignedToUser)
                .ToListAsync();

            return tickets;
        }


        /// <summary>
        /// Returns a TicketDto object for a specific ticket identified by the id given as input.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TicketDto> GetTicketByIdAsync(Guid id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.TicketType)
                .Include(t => t.TicketStatus)
                .Include(t => t.CreatedByUser)
                .Include(t => t.UpdatedByUser)
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticket == null)
            {
                throw new ArgumentException($"Ticket with id {id} not found");
            }

            return _mapper.Map<TicketDto>(ticket);
        }


        /// <summary>
        /// Creates a new ticket in the database based on a TicketDto object given as input, and returns the new ticket mapped to a TicketDto object.
        /// </summary>
        /// <param name="ticketDto"></param>
        /// <returns></returns>
        public async Task<TicketDto> CreateTicketAsync(TicketDto ticketDto)
        {
            var ticket = _mapper.Map<Ticket>(ticketDto);

            _context.Tickets.Add(ticket);

            await _context.SaveChangesAsync();

            return _mapper.Map<TicketDto>(ticket);
        }

        /// <summary>
        /// Updates a ticket in the database based on a TicketDto object given as input, and saves the changes.
        /// </summary>
        /// <param name="ticketDto"></param>
        /// <returns></returns>
        public async Task UpdateTicketAsync(TicketDto ticketDto)
        {
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.TicketId == ticketDto.TicketId);

            if (ticket == null)
            {
                throw new ArgumentException($"Ticket with id {ticketDto.TicketId} not found");
            }

            _mapper.Map(ticketDto, ticket);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a ticket from the database based on the ticket id given as input, and saves the changes.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task DeleteTicketAsync(Guid id)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticket == null)
            {
                throw new ArgumentException($"Ticket with id {id} not found");
            }

            _context.Tickets.Remove(ticket);

            await _context.SaveChangesAsync();
        }
    }
}
