using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryTests.TestMappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserTicketSystemCore.Interfaces;
using UserTicketSystemCore.Models;
using UserTicketSystemCore.Models.Dtos;
using UserTicketSystemCore.Models.LookUpModels;
using UserTicketSystemData;
using UserTicketSystemData.Repositories;

namespace RepositoryTests
{
    [TestClass]
    public class TicketRepositoryTests
    {
        private IMapper _mapper;
        private UserTicketSystemContext _context;
        private IUserRepository _userRepository;
        private ITicketRepository _ticketRepository;
        private IUserHierarchyRepository _userHierarchyRepository;
        private Mock<UserTicketSystemContext> _mockContext;
        private Mock<DbContext> _mockDbContext;

        [TestInitialize]
        public void ClassInitialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CustomMapping());
            });
            _mapper = config.CreateMapper();

            var options = new DbContextOptionsBuilder<UserTicketSystemContext>()
                .UseInMemoryDatabase(databaseName: "UserTicketSystemTestDatabase")
                .Options;
            _context = new UserTicketSystemContext(options);

            _userRepository = new UserRepository(_context, _mapper, _userHierarchyRepository);
            _ticketRepository = new TicketRepository(_context, _mapper, _userRepository);
            _mockContext = new Mock<UserTicketSystemContext>();
            _mockDbContext = new Mock<DbContext>();
            if (_context.Users.Count() == 0)
            {
                setupContextTestData();
            }
        }
        [TestCleanup]
        public void TestCleanup()
        {
            // Dispose the context to clear the database
            _context.Dispose();
        }

        private void setupContextTestData() 
        {

            var user = new User { Username = "testuser", Id = 1 };
            var user2 = new User { Username = "testuser2", Id = 2 };
            _context.Users.AddRange(user,user2 );

            var ticket1 = new Ticket
            {
                Title = "Ticket 1",
                Description = "Ticket 1 Description",
                AssignedToUser = user,
                AssignedToId = user.Id
            };
            var ticket2 = new Ticket
            {
                Title = "Ticket 2",
                Description = "Ticket 2 Description",
                AssignedToUser = user,
                AssignedToId = user.Id
            };
            var ticket3 = new Ticket
            {
                Title = "Ticket 3",
                Description = "Ticket 3 Description",
                AssignedToUser = user2,
                AssignedToId = user2.Id
            };
            var ticket4 = new Ticket
            {
                Title = "Ticket 4",
                Description = "Ticket 4 Description"
            };


            var ticketStatus = new TicketStatus { Name = "In progress" };
            var ticketType = new TicketType { Name = "Bug report" };
            _context.TicketStatuses.Add(ticketStatus);
            _context.TicketTypes.Add(ticketType);

            ticket1.TicketStatus = ticketStatus;
            ticket1.CreatedBy = user.Id;
            ticket1.UpdatedBy = user.Id;
            ticket1.TicketType = ticketType;

            ticket2.TicketStatus = ticketStatus;
            ticket2.TicketType = ticketType;
            ticket2.CreatedBy = user.Id;
            ticket2.UpdatedBy = user.Id;

            ticket3.TicketStatus = ticketStatus;
            ticket3.CreatedBy = user.Id;
            ticket3.UpdatedBy = user.Id;
            ticket3.TicketType = ticketType;


            ticket4.TicketStatus = ticketStatus;
            ticket4.CreatedBy = user.Id;
            ticket4.UpdatedBy = user.Id;
            ticket4.TicketType = ticketType;

            _context.Tickets.AddRange(ticket1, ticket2, ticket3,ticket4);
             _context.SaveChanges();
        }       
        [TestMethod]
        public async Task GetAllTicketsAsync_ReturnsAllTickets()
        {
            // Arrange
            var filters = new TicketLookUpParameters();

            // Act
            var result = await _ticketRepository.GetAllTicketsAsync(filters);

            // Assert
            Assert.AreEqual(4, result.Count());
        }

        [TestMethod]
        public async Task GetAllTicketsForUserAsync_ReturnsAllUserTickets()
        {
            // Arrange
            var filters = new TicketLookUpParameters();
            var userId = 1;

            // Act
            var result = await _ticketRepository.GetAllTicketsForUserAsync(filters, userId);

            // Assert
            Assert.AreEqual(2, result.Count());
        }


        [TestMethod]
        public async Task GetAllTicketsUnderManagerAsync_ReturnsAllManagerTicketsAndSubordinates()
        {
            // Arrange
            var filters = new TicketLookUpParameters();
            int managerId = 1;
            var mockManager = new List<UserHierarchy> { new UserHierarchy { Id = 1,UserId = 1, ReportingUserId =2 }, new UserHierarchy { Id = 1, UserId = 1, ReportingUserId = 3 } };

            var userSubordinates = new List<UserDto>
            {
                new UserDto { Id = 2,  ReportsToId= managerId },
                new UserDto { Id = 3, ReportsToId = managerId }
            };

                    var mockUserRepository = new Mock<IUserRepository>();
                    mockUserRepository.Setup(repo => repo.GetAllManagerSubortinates(managerId))
                                      .ReturnsAsync(userSubordinates);


                    var repository = new TicketRepository(_context, _mapper, mockUserRepository.Object);

                    // Act
                    var result = await repository.GetAllTicketsUnderManagerAsync(filters, managerId);

                    // Assert
                    Assert.AreEqual(3, result.Count());
                }

         }


}
