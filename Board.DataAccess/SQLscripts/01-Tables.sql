-- 1. Create Users table (Stores Auth0 identity)
CREATE TABLE Users (
    Id VARCHAR(64) NOT NULL PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL UNIQUE, 
    DisplayName NVARCHAR(20) NOT NULL CHECK (LEN(DisplayName) >= 3),
    CreatedAt DATETIME2 NOT NULL
);
GO

-- 2. Create Boards table (Projects / Workspaces)
CREATE TABLE Boards (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL CHECK (LEN(Title) >= 3),
    Description NVARCHAR(500) NOT NULL CHECK (LEN(Description) >= 10),
    CreatedAt DATETIME2 NOT NULL
);
GO

-- 3. Create BoardMembers table (Handles Roles: Admin vs User)
-- This allows a user to be an Admin in one project and a Member in another
CREATE TABLE BoardMembers (
    BoardId BIGINT NOT NULL FOREIGN KEY REFERENCES Boards(Id) ON DELETE CASCADE,
    UserId VARCHAR(64) NOT NULL FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Admin', 'User')), 
    PRIMARY KEY (BoardId, UserId)
);
GO

-- 4. Create Columns table (Linked to a specific Board)
CREATE TABLE Columns (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL CHECK (LEN(Name) >= 3),
    Description NVARCHAR(200) NOT NULL CHECK (LEN(Description) >= 10),
    Position INT NOT NULL,
    BoardId BIGINT NOT NULL FOREIGN KEY REFERENCES Boards(Id) ON DELETE CASCADE
);
GO

-- 5. Create Issues table (Linked to a specific Column)
CREATE TABLE Issues (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL CHECK (LEN(Title) >= 3),
    Description NVARCHAR(2000) NOT NULL CHECK (LEN(Description) >= 10),
    DueDate DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL,
    PositionInColumn BIGINT NOT NULL,
    ColumnId BIGINT NOT NULL FOREIGN KEY REFERENCES Columns(Id) ON DELETE CASCADE,
    CreatorId VARCHAR(64) NOT NULL FOREIGN KEY REFERENCES Users(Id),
    AssigneeId VARCHAR(64) NULL FOREIGN KEY REFERENCES Users(Id)
);
GO

-- ==========================================
-- POPULATE INITIAL DATA (English Only)
-- ==========================================

-- Insert Users
INSERT INTO Users (Id, Email, DisplayName, CreatedAt) VALUES
('auth0|698b956080889e5401cef7c5','user1@example.com', 'Taras Shevchenko', SYSDATETIME()),
('auth0|698b9bd69f764e2999518960','user2@example.com', 'Ivan Franko', SYSDATETIME()),
('auth0|698b9bfb9f764e2999518993','user3@example.com', 'Lesya Ukrainka', SYSDATETIME());
GO

-- Insert Boards (Projects)
INSERT INTO Boards (Title, Description, CreatedAt) VALUES 
('Main Development Board', 'Core application development workspace', SYSDATETIME()),
('Marketing Campaign', 'Planning for the new product launch', SYSDATETIME());
GO

-- Assign Roles (Taras is Admin of Board 1, Ivan is a Member)
INSERT INTO BoardMembers (BoardId, UserId, Role) VALUES 
(1, 'auth0|698b956080889e5401cef7c5', 'Admin'),
(1, 'auth0|698b9bd69f764e2999518960', 'User'),
(2, 'auth0|698b9bd69f764e2999518960', 'Admin'); -- Ivan is Admin of the Marketing board
GO

-- Insert Columns for Board 1
INSERT INTO Columns (Name, Description, Position, BoardId) VALUES
('ToDo', 'Tasks ready to be started', 0, 1),
('In Progress', 'Tasks currently being worked on', 1, 1),
('Testing', 'Tasks waiting for QA review', 2, 1),
('Done', 'Completed and verified tasks', 3, 1);
GO

--- 6. Populate Issues table (Updated with 10 tasks)
INSERT INTO Issues (Title, Description, DueDate, CreatedAt, PositionInColumn, ColumnId, CreatorId, AssigneeId) VALUES
-- Column 1: ToDo (ColumnId = 1)
('Implement RBAC', 'Set up Role-Based Access Control using the new tables.', '2025-04-01', SYSDATETIME(), 0, 1, 'auth0|698b956080889e5401cef7c5', 'auth0|698b9bd69f764e2999518960'),
('Design UI wireframes', 'Create initial design wireframes for the dashboard.', '2025-03-10', SYSDATETIME(), 1, 1, 'auth0|698b956080889e5401cef7c5', 'auth0|698b9bd69f764e2999518960'),
('User Profile Page', 'Create a page where users can update their display names and emails.', '2025-04-15', SYSDATETIME(), 2, 1, 'auth0|698b9bfb9f764e2999518993', NULL),
-- Column 2: In Progress (ColumnId = 2)
('Database Optimization', 'Add indexes to foreign key columns for better performance.', NULL, SYSDATETIME(), 0, 2, 'auth0|698b9bd69f764e2999518960', 'auth0|698b956080889e5401cef7c5'),
('Auth0 Integration', 'Finalize the login flow and handle redirect callback parameters.', '2025-03-20', SYSDATETIME(), 1, 2, 'auth0|698b956080889e5401cef7c5', 'auth0|698b956080889e5401cef7c5'),
('API Rate Limiting', 'Implement basic rate limiting to prevent API abuse on public endpoints.', NULL, SYSDATETIME(), 2, 2, 'auth0|698b9bd69f764e2999518960', 'auth0|698b9bfb9f764e2999518993'),
-- Column 3: Testing (ColumnId = 3)
('Unit Testing: Auth Service', 'Write unit tests for the authentication and token validation service.', '2025-03-12', SYSDATETIME(), 0, 3, 'auth0|698b9bfb9f764e2999518993', 'auth0|698b9bfb9f764e2999518993'),
('Bug Fix: Drag & Drop', 'Fix the issue where items sometimes flicker during column movement.', '2025-03-05', SYSDATETIME(), 1, 3, 'auth0|698b956080889e5401cef7c5', 'auth0|698b9bd69f764e2999518960'),
-- Column 4: Done (ColumnId = 4)
('Initial Project Setup', 'Create the React application and set up basic folder structure.', '2025-02-20', SYSDATETIME(), 0, 4, 'auth0|698b956080889e5401cef7c5', 'auth0|698b956080889e5401cef7c5'),
('Define SQL Schema', 'Draft the initial database schema for Users, Columns, and Issues.', '2025-02-22', SYSDATETIME(), 1, 4, 'auth0|698b9bd69f764e2999518960', 'auth0|698b9bd69f764e2999518960');
GO