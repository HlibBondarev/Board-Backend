-- 1. Create Users table
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL UNIQUE, 
    DisplayName NVARCHAR(20) NOT NULL CHECK (LEN(DisplayName) >= 3),
	CreatedAt DATETIME2 NOT NULL
);
GO

-- 2. Create Columns table
CREATE TABLE Columns (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL CHECK (LEN(Name) >= 3),
    Description NVARCHAR(200) NOT NULL CHECK (LEN(Description) >= 10),
	Position INT NOT NULL,
	UserId  INT NOT NULL FOREIGN KEY REFERENCES Users(Id)
);
GO

-- 3. Create Issues table
CREATE TABLE Issues (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL CHECK (LEN(Title) >= 3),
    Description NVARCHAR(2000) NOT NULL CHECK (LEN(Description) >= 10),
    DueDate DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL,
    PositionInColumn INT NOT NULL,
	ColumnId INT NOT NULL FOREIGN KEY REFERENCES Columns(Id),
    CreatorId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    AssigneeId INT NULL FOREIGN KEY REFERENCES Users(Id)
);
GO

-- 4. Populate Users table
INSERT INTO Users (Email, DisplayName, CreatedAt) VALUES
('user1@example.com', 'Ivan Franko', SYSDATETIME()),
('user2@example.com', 'Lesya Ukrainka', SYSDATETIME());
GO

-- 5. Populate Columns table
INSERT INTO Columns (Name, Description, Position, UserId) VALUES
('ToDo', 'For work in ToDo', 1, 1),
('In Progress', 'For work in Progres', 2, 1),
('Testing', 'For work in Testing', 3, 2),
('Done', 'For work is Done', 4, 2),
('Archived', 'For work is archived', 5, 2);
GO

-- 6. Populate Issues table
INSERT INTO Issues (Title, Description, DueDate, CreatedAt, PositionInColumn, ColumnId, CreatorId, AssigneeId) VALUES
('Design UI wireframes', 'Create initial design wireframes for the new application interface.', '2025-03-10', SYSDATETIME(), 1, 1, 1, 2),
('Set up database', 'Configure MSSQL database and create necessary tables and constraints.', NULL, SYSDATETIME(), 1, 2, 2, 1),
('Develop User model', 'Write the C# class for the User entity with Dapper integration.', '2025-03-15', SYSDATETIME(), 1, 2, 1, 1),
('Implement task creation', 'Develop functionality for creating new tasks via the API endpoint.', '2025-03-20', SYSDATETIME(), 1, 3, 2, 2),
('Testing user login', 'Perform unit and integration tests for the user authentication module.', NULL, SYSDATETIME(), 2, 3, 1, 1),
('Fix bug in column sorting', 'Resolve an issue where columns are not sorted correctly in the UI.', '2025-03-05', SYSDATETIME(), 1, 4, 2, NULL),
('Write API documentation', 'Document all API endpoints for tasks using Swagger.', '2025-03-25', SYSDATETIME(), 2, 4, 1, 2),
('Plan next sprint', 'Hold a meeting to plan the tasks for the next development sprint.', NULL, SYSDATETIME(), 3, 1, 1, 1);
GO