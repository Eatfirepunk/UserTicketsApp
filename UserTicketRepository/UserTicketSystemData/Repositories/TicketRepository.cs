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

        public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync(TicketLookUpParameters filters)
        {
            var ticketsQuery = _context.Tickets.AsQueryable();
            
            var tickets = await BuildTicketFilters(filters, ticketsQuery);

            return _mapper.Map<IEnumerable<TicketDto>>(tickets);
        }

        // separated method in case retrieving only one user tickets has additional logic
        public async Task<IEnumerable<TicketDto>> GetAllTicketsForUserAsync(TicketLookUpParameters filters, int userId)
        {
            var ticketsQuery = _context.Tickets.Where(x=> x.AssignedToId == userId);
            var tickets = await BuildTicketFilters(filters, ticketsQuery);

            return _mapper.Map<IEnumerable<TicketDto>>(tickets);
        }


        public async Task<IEnumerable<TicketDto>> GetAllTicketsUnderManagerAsync(TicketLookUpParameters filters, int managerId)
        {
            var usersUnderManager = await _userRepository.GetAllManagerSubortinates(managerId);
            var userIds = usersUnderManager.Select(x => x.Id).Distinct().ToList();
            var ticketsQuery = _context.Tickets.Where(x => (x.AssignedToId.HasValue && userIds.Contains(x.AssignedToId.Value)) || x.AssignedToId == managerId);

            var tickets = await BuildTicketFilters(filters, ticketsQuery);

            return _mapper.Map<IEnumerable<TicketDto>>(tickets);
        }

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

        public async Task<TicketDto> CreateTicketAsync(TicketDto ticketDto)
        {
            var ticket = _mapper.Map<Ticket>(ticketDto);

            _context.Tickets.Add(ticket);

            await _context.SaveChangesAsync();

            return _mapper.Map<TicketDto>(ticket);
        }

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
