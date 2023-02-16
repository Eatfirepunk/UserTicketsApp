using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Interfaces;
using UserTicketSystemCore.Models.Dtos;
using UserTicketSystemCore.Models.LookUpModels;
using UserTicketSystemCore.Services;

namespace RepositoryTests
{
    [TestClass]
    public class TicketServiceTests
    {
        private Mock<ITicketRepository> _ticketRepositoryMock;
        private TicketService _ticketService;

        [TestInitialize]
        public void Initialize()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _ticketService = new TicketService(_ticketRepositoryMock.Object);
        }

        [TestMethod]
        public async Task GetAllTicketsAsync_ReturnsListOfTickets()
        {
            // Arrange
            var filters = new TicketLookUpParameters();
            var tickets = new List<TicketDto>
        {
            new TicketDto { TicketId = Guid.NewGuid(), Title = "Ticket 1", Description = "Description 1" },
            new TicketDto { TicketId = Guid.NewGuid(), Title = "Ticket 2", Description = "Description 2" },
            new TicketDto { TicketId = Guid.NewGuid(), Title = "Ticket 3", Description = "Description 3" }
        };
            _ticketRepositoryMock.Setup(r => r.GetAllTicketsAsync(filters)).ReturnsAsync(tickets);

            // Act
            var result = await _ticketService.GetAllTicketsAsync(filters);

            // Assert
            Assert.AreEqual(tickets.Count, result.Count());
            CollectionAssert.AreEqual(tickets.Select(t => t.TicketId).ToList(), result.Select(t => t.TicketId).ToList());
        }

        [TestMethod]
        public async Task GetAllTicketsForUserAsync_ReturnsListOfTickets()
        {
            // Arrange
            var filters = new TicketLookUpParameters();
            var userId = 1;
            var tickets = new List<TicketDto>
        {
            new TicketDto { TicketId = Guid.NewGuid(), Title = "Ticket 1", Description = "Description 1" },
            new TicketDto { TicketId = Guid.NewGuid(), Title = "Ticket 2", Description = "Description 2" },
            new TicketDto { TicketId = Guid.NewGuid(), Title = "Ticket 3", Description = "Description 3" }
        };
            _ticketRepositoryMock.Setup(r => r.GetAllTicketsForUserAsync(filters, userId)).ReturnsAsync(tickets);

            // Act
            var result = await _ticketService.GetAllTicketsForUserAsync(filters, userId);

            // Assert
            Assert.AreEqual(tickets.Count, result.Count());
            CollectionAssert.AreEqual(tickets.Select(t => t.TicketId).ToList(), result.Select(t => t.TicketId).ToList());
        }

        [TestMethod]
        public async Task GetTicketByIdAsync_ReturnsTicket()
        {
            // Arrange
            var ticketId = Guid.NewGuid();
            var ticket = new TicketDto { TicketId = ticketId, Title = "Ticket 1", Description = "Description 1" };
            _ticketRepositoryMock.Setup(r => r.GetTicketByIdAsync(ticketId)).ReturnsAsync(ticket);

            // Act
            var result = await _ticketService.GetTicketByIdAsync(ticketId);

            // Assert
            Assert.AreEqual(ticketId, result.TicketId);
            Assert.AreEqual(ticket.Title, result.Title);
            Assert.AreEqual(ticket.Description, result.Description);
        }

        [TestMethod]
        public async Task CreateTicketAsync_CreatesTicket()
        {
            // Arrange
            var ticketDto = new TicketDto { Title = "Ticket 1", Description = "Description 1" };
            var createdTicketDto = new TicketDto { TicketId = Guid.NewGuid(), Title = "Ticket 1", Description = "Description 1" };
            _ticketRepositoryMock.Setup(r => r.CreateTicketAsync(ticketDto)).ReturnsAsync(createdTicketDto);

            // Act
            var result = await _ticketService.CreateTicketAsync(ticketDto);


            // Assert
            Assert.IsNotNull(result.TicketId);
            Assert.AreEqual(ticketDto.Title, result.Title);
            Assert.AreEqual(ticketDto.Description, result.Description);
        }
    }
}
