using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Interfaces;
using UserTicketSystemCore.Models.Dtos;
using UserTicketSystemCore.Models.LookUpModels;
using UserTicketSystemCore.Services.Abstractions;

namespace UserTicketSystemCore.Services
{
    //The repository is not called directly instead a service was created to follow the Open/Closed principle so logic gets added here and not on the upper layers in case you need to extend the functionality
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
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
