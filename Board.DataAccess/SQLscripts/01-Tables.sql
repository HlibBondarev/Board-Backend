-- 1. Create Users table
CREATE TABLE Users (
    Id VARCHAR(64) NOT NULL PRIMARY KEY,
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
	UserId  VARCHAR(64) NOT NULL FOREIGN KEY REFERENCES Users(Id)
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
    CreatorId VARCHAR(64) NOT NULL FOREIGN KEY REFERENCES Users(Id),
    AssigneeId VARCHAR(64) NULL FOREIGN KEY REFERENCES Users(Id)
);
GO

-- 4. Populate Users table
INSERT INTO Users (Id, Email, DisplayName, CreatedAt) VALUES
('auth0|698b956080889e5401cef7c5','user1@example.com', 'Taras Shevchenko', SYSDATETIME()),
('auth0|698b9bd69f764e2999518960','user2@example.com', 'Ivan Franko', SYSDATETIME()),
('auth0|698b9bfb9f764e2999518993','user3@example.com', 'Lesya Ukrainka', SYSDATETIME());
GO

-- 5. Populate Columns table
INSERT INTO Columns (Name, Description, Position, UserId) VALUES
('ToDo', 'For work in ToDo', 1, 'auth0|698b956080889e5401cef7c5'),
('In Progress', 'For work in Progres', 2, 'auth0|698b956080889e5401cef7c5'),
('Testing', 'For work in Testing', 3, 'auth0|698b9bd69f764e2999518960'),
('Done', 'For work is Done', 4, 'auth0|698b9bd69f764e2999518960'),
('Archived', 'For work is Archived', 5, 'auth0|698b9bd69f764e2999518960');
GO

-- 6. Populate Issues table
INSERT INTO Issues (Title, Description, DueDate, CreatedAt, PositionInColumn, ColumnId, CreatorId, AssigneeId) VALUES
('Design UI wireframes', 'Create initial design wireframes for the new application interface.', '2025-03-10', SYSDATETIME(), 1, 1, 'auth0|698b956080889e5401cef7c5', 'auth0|698b9bd69f764e2999518960'),
('Set up database', 'Configure MSSQL database and create necessary tables and constraints.', NULL, SYSDATETIME(), 1, 2, 'auth0|698b9bd69f764e2999518960', 'auth0|698b956080889e5401cef7c5'),
('Develop User model', 'Write the C# class for the User entity with Dapper integration.', '2025-03-15', SYSDATETIME(), 1, 2, 'auth0|698b956080889e5401cef7c5', 'auth0|698b956080889e5401cef7c5'),
('Implement task creation', 'Develop functionality for creating new tasks via the API endpoint.', '2025-03-20', SYSDATETIME(), 1, 3, 'auth0|698b9bd69f764e2999518960', 'auth0|698b9bd69f764e2999518960'),
('Testing user login', 'Perform unit and integration tests for the user authentication module.', NULL, SYSDATETIME(), 2, 3, 'auth0|698b956080889e5401cef7c5', 'auth0|698b956080889e5401cef7c5'),
('Fix bug in column sorting', 'Resolve an issue where columns are not sorted correctly in the UI.', '2025-03-05', SYSDATETIME(), 1, 4, 'auth0|698b9bd69f764e2999518960', NULL),
('Write API documentation', 'Document all API endpoints for tasks using Swagger.', '2025-03-25', SYSDATETIME(), 2, 4, 'auth0|698b956080889e5401cef7c5', 'auth0|698b9bd69f764e2999518960'),
('Plan next sprint', 'Hold a meeting to plan the tasks for the next development sprint.', NULL, SYSDATETIME(), 3, 1, 'auth0|698b956080889e5401cef7c5', 'auth0|698b956080889e5401cef7c5');
GO