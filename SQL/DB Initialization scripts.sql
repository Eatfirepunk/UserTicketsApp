CREATE TABLE Roles (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
)

CREATE TABLE Users (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Username VARCHAR(50) NOT NULL,
    PasswordHash VARBINARY(256) NOT NULL,
    PasswordSalt VARBINARY(128) NOT NULL,
	JwtSecret VARCHAR(512) NOT NULL
)

CREATE TABLE [dbo].[UserRole]
(
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]),
    CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([Id])
)

CREATE TABLE UserHierarchies (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId INT NOT NULL,
    ReportingUserId INT NOT NULL,
    CONSTRAINT FK_UserHierarchy_UserId FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_UserHierarchy_ReportingUserId FOREIGN KEY (ReportingUserId) REFERENCES Users(Id)
)



INSERT INTO Roles (Name) VALUES ('Admin')
INSERT INTO Roles (Name) VALUES ('Developer/Tester')
INSERT INTO Roles (Name) VALUES ('Lead/Manager')



CREATE TABLE TicketTypes (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
)

INSERT INTO TicketTypes (Name) VALUES ('Bug')
INSERT INTO TicketTypes (Name) VALUES ('Feature Request')
INSERT INTO TicketTypes (Name) VALUES ('Technical Issue')


CREATE TABLE TicketStatuses (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
)


INSERT INTO TicketStatuses (Name) VALUES ('New')
INSERT INTO TicketStatuses (Name) VALUES ('In Progress')
INSERT INTO TicketStatuses (Name) VALUES ('Approved')
INSERT INTO TicketStatuses (Name) VALUES ('Rejected')


CREATE TABLE Tickets (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TicketId UNIQUEIDENTIFIER NOT NULL,
    Title VARCHAR(100) NOT NULL,
    Description VARCHAR(1000) NOT NULL,
    TicketTypeId INT NOT NULL,
    CreatedDatetime DATETIME NOT NULL,
    StatusId INT NOT NULL,
	CreatedBy INT NOT NULL,
	UpdatedBy INT NULL,
	AssignedTo INT NOT NULL,
    CONSTRAINT FK_Tickets_TicketTypes FOREIGN KEY (TicketTypeId) REFERENCES TicketTypes(Id),
    CONSTRAINT FK_Tickets_TicketStatuses FOREIGN KEY (StatusId) REFERENCES TicketStatuses(Id),
	CONSTRAINT FK_Tickets_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(Id),
    CONSTRAINT FK_Tickets_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES Users(Id),
    CONSTRAINT FK_Tickets_AssignedTo FOREIGN KEY (AssignedTo) REFERENCES Users(Id)
)


INSERT INTO Users (Email, Username, PasswordHash, PasswordSalt, JwtSecret)
VALUES ('john.doe@example.com', 'johndoe', 0x48765C2F37BCC65E6D92E6C09F7F0C6FEFBD7E2F0CFA64B3B16F3C3F9E906B1F, 0x8FA3B3EEF110EBFB, 'jwtsecret1'),
('jane.doe@example.com', 'janedoe', 0x5C28EDD1F251EFBCB032CBE5C5DB5CE5D10A4919E9D3C0B08DC3B94B3C0CC306, 0xD67DC4C4D4E4E705, 'jwtsecret2')

INSERT INTO UserRole (UserId, RoleId)
VALUES (1, 1),
(1, 2),
(2, 1),
(2, 3)

INSERT INTO UserHierarchies (UserId, ReportingUserId)
VALUES (2, 1)


