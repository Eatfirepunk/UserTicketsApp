using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserTicketSystemCore.Interfaces;
using UserTicketSystemCore.Models.Dtos;
using UserTicketSystemCore.Models.LookUpModels;
using UserTicketSystemCore.Services.Abstractions;

namespace TicketsMicroservice.Services
{
    public class TicketsService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketsService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync(TicketLookUpParameters filters)
        {
            return await _ticketRepository.GetAllTicketsAsync(filters);
        }

        public async Task<IEnumerable<TicketDto>> GetAllTicketsForUserAsync(TicketLookUpParameters filters, int userId)
        {
            return await _ticketRepository.GetAllTicketsForUserAsync(filters, userId);
        }

        public async Task<TicketDto> GetTicketByIdAsync(Guid id)
        {
            return await _ticketRepository.GetTicketByIdAsync(id);
        }

        public async Task<TicketDto> CreateTicketAsync(TicketDto ticketDto)
        {
            return await _ticketRepository.CreateTicketAsync(ticketDto);
        }

        public async Task UpdateTicketAsync(TicketDto ticketDto)
        {
            await _ticketRepository.UpdateTicketAsync(ticketDto);
        }

        public async Task DeleteTicketAsync(Guid id)
        {
            await _ticketRepository.DeleteTicketAsync(id);
        }

        public async Task<IEnumerable<TicketDto>> GetAllTicketsUnderLeadManagerAsync(TicketLookUpParameters filters, int userId)
        {
            return await _ticketRepository.GetAllTicketsUnderManagerAsync(filters, userId);
        }
    }
}
