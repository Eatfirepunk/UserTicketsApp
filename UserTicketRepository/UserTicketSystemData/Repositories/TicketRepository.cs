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

        public TicketRepository(UserTicketSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync(TicketLookUpParameters filters)
        {
            var ticketsQuery = _context.Tickets.AsQueryable();
            BuildTicketFilters(filters, ticketsQuery);
            var tickets = await ticketsQuery
                .Include(t => t.TicketType)
                .Include(t => t.TicketStatus)
                .Include(t => t.CreatedByUser)
                .Include(t => t.UpdatedByUser)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TicketDto>>(tickets);
        }


        // separated method in case retrieving only one user tickets has additional logic
        public async Task<IEnumerable<TicketDto>> GetAllTicketsForUserAsync(TicketLookUpParameters filters, int userId)
        {
            var ticketsQuery = _context.Tickets.Where(x=> x.Id == userId);

            BuildTicketFilters(filters,ticketsQuery);

            var tickets = await ticketsQuery
                .Include(t => t.TicketType)
                .Include(t => t.TicketStatus)
                .Include(t => t.CreatedByUser)
                .Include(t => t.UpdatedByUser)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TicketDto>>(tickets);
        }

        private void BuildTicketFilters(TicketLookUpParameters filters, IQueryable<Ticket> ticketsQuery) 
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
        }

        public async Task<TicketDto> GetTicketByIdAsync(Guid id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.TicketType)
                .Include(t => t.TicketStatus)
                .Include(t => t.CreatedByUser)
                .Include(t => t.UpdatedByUser)
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
