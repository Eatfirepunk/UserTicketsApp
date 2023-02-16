using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersMicroService.Services;
using UserTicketSystemCore;
using UserTicketSystemCore.Interfaces;
using UserTicketSystemCore.Models.Dtos;

namespace UserTests
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IUserHierarchyRepository> _userHierarchyRepositoryMock;
        private UserService _userService;

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userHierarchyRepositoryMock = new Mock<IUserHierarchyRepository>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _userHierarchyRepositoryMock.Object
            );
        }

        [TestMethod]
        public async Task CreateUserAsync_WithNonExistingEmailAndReportsToId_ReturnsCreatedUser()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "testuser@example.com",
                Password = "password",
                ReportsToId = null
            };
            var newUser = new UserDto
            {
                Id = 1,
                Email = "testuser@example.com",
            };
            _userRepositoryMock.Setup(ur => ur.GetUserByEmailAsync(loginDto.Email))
                .ReturnsAsync((UserDto)null);
            _userRepositoryMock.Setup(ur => ur.CreateUserAsync(loginDto))
                .ReturnsAsync(newUser);
            _userRepositoryMock.Setup(ur => ur.GetUserByIdAsync(newUser.Id))
                .ReturnsAsync(newUser);

            // Act
            var result = await _userService.CreateUserAsync(loginDto);

            // Assert
            Assert.AreEqual(newUser.Id, result.Id);
            Assert.AreEqual(newUser.Email, result.Email);
            _userRepositoryMock.Verify(ur => ur.GetUserByEmailAsync(loginDto.Email), Times.Once);
            _userRepositoryMock.Verify(ur => ur.CreateUserAsync(loginDto), Times.Once);
            _userRepositoryMock.Verify(ur => ur.GetUserByIdAsync(newUser.Id), Times.Once);
            _userHierarchyRepositoryMock.Verify(ur => ur.AddUserHierarchyAsync(It.IsAny<UserHierarchyDto>()), Times.Never);
        }

        [TestMethod]
        public async Task CreateUserAsync_WithExistingEmail_ThrowsException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "testuser@example.com",
                Password = "password",
                ReportsToId = null
            };
            var existingUser = new UserDto
            {
                Id = 1,
                Email = "testuser@example.com"
            };
            _userRepositoryMock.Setup(ur => ur.GetUserByEmailAsync(loginDto.Email))
                .ReturnsAsync(existingUser);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => _userService.CreateUserAsync(loginDto));
            _userRepositoryMock.Verify(ur => ur.GetUserByEmailAsync(loginDto.Email), Times.Once);
            _userRepositoryMock.Verify(ur => ur.CreateUserAsync(loginDto), Times.Never);
            _userRepositoryMock.Verify(ur => ur.GetUserByIdAsync(It.IsAny<int>()), Times.Never);
            _userHierarchyRepositoryMock.Verify(ur => ur.AddUserHierarchyAsync(It.IsAny<UserHierarchyDto>()), Times.Never);
        }

        [TestMethod]
        public async Task CreateUserAsync_AddsUserHierarchy_WhenReportsToIdIsNotNull()
        {
            // Arrange
            var newUser = new UserDto { Id = 3, Email = "test3@test.com" };
            var loginDto = new LoginDto { Email = "test3@test.com", Password = "password", ReportsToId = 1 };
            _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(loginDto.Email))
                .ReturnsAsync((UserDto)null);
            _userRepositoryMock.Setup(r => r.CreateUserAsync(loginDto))
                .ReturnsAsync(newUser);
            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(newUser.Id))
                .ReturnsAsync(newUser);
            var userService = new UserService(_userRepositoryMock.Object, _userHierarchyRepositoryMock.Object);

            // Act
            await userService.CreateUserAsync(loginDto);

            // Assert
            _userHierarchyRepositoryMock.Verify(r => r.AddUserHierarchyAsync(It.Is<UserHierarchyDto>(
                h => h.UserId == newUser.Id && h.ReportingUserId == loginDto.ReportsToId)));
        }

        [TestMethod]
        public async Task DeleteUserAsync_CallsUserRepositoryDeleteUserAsync()
        {
            // Arrange
            var userId = 1;
            var userService = new UserService(_userRepositoryMock.Object, _userHierarchyRepositoryMock.Object);

            // Act
            await userService.DeleteUserAsync(userId);

            // Assert
            _userRepositoryMock.Verify(r => r.DeleteUserAsync(userId));
        }

        [TestMethod]
        public async Task GetUsersAsync_CallsUserRepositoryAndReturnsListOfUsers()
        {
            // Arrange
            var users = new List<UserDto>
        {
            new UserDto { Id = 1, Username = "User1" },
            new UserDto { Id = 2, Username = "User2" },
            new UserDto { Id = 3, Username = "User3" }
        };
            _userRepositoryMock.Setup(x => x.GetUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            _userRepositoryMock.Verify(x => x.GetUsersAsync(), Times.Once);
            CollectionAssert.AreEqual(users, result.ToList());
        }

        [TestMethod]
        public async Task LoginAsync_CallsUserRepositoryAndReturnsUserDto()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@test.com", Password = "password" };
            var userDto = new UserDto { Id = 1, Username = "User1" };
            _userRepositoryMock.Setup(x => x.LoginAsync(loginDto)).ReturnsAsync(userDto);

            // Act
            var result = await _userService.LoginAsync(loginDto);

            // Assert
            _userRepositoryMock.Verify(x => x.LoginAsync(loginDto), Times.Once);
            Assert.AreEqual(userDto, result);
        }

        [TestMethod]
        public async Task LoginAsync_ThrowsExceptionWhenInvalidCredentialsProvided()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "nonexistentuser@example.com",
                Password = "wrongpassword"
            };
            _userRepositoryMock.Setup(ur => ur.LoginAsync(loginDto)).ReturnsAsync((UserDto)null);

            //Act
            var failedLogin = await _userService.LoginAsync(loginDto);
            //Assert
            Assert.IsNull(failedLogin);
        }

        [TestMethod]
        public async Task UpdateUserAsync_CallsUserRepositoryAndUpdateHierarchy()
        {
            // Arrange
            var userDto = new UserDto { Id = 1, Username = "User1", ReportsToId = 2 };

            // Act
            await _userService.UpdateUserAsync(userDto);

            // Assert
            _userRepositoryMock.Verify(x => x.UpdateUserAsync(userDto), Times.Once);
            if (userDto.ReportsToId.HasValue)
            {
                _userHierarchyRepositoryMock.Verify(x => x.UpdateUserHierarchyAsync(It.IsAny<UserHierarchyDto>()), Times.Once);
            }
            else
            {
                _userHierarchyRepositoryMock.Verify(x => x.UpdateUserHierarchyAsync(It.IsAny<UserHierarchyDto>()), Times.Never);
            }
        }
    }
}
