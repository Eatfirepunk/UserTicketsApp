using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Interfaces;
using UserTicketSystemCore.Models;
using UserTicketSystemCore.Models.Dtos;

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

        public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync()
        {
            var tickets = await _context.Tickets
                .Include(t => t.TicketType)
                .Include(t => t.TicketStatus)
                .Include(t => t.CreatedByUser)
                .Include(t => t.UpdatedByUser)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TicketDto>>(tickets);
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
