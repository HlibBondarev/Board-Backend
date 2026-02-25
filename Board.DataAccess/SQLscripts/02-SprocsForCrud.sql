-- Users table stores user identities from Auth0. The Id is the unique identifier provided by Auth0 
-- (e.g., "auth0|698b956080889e5401cef7c5").
-- Create a new user with Auth0 ID and application-provided timestamp
CREATE PROCEDURE sp_Users_Create
    @Id VARCHAR(64),
    @Email NVARCHAR(255),
    @DisplayName NVARCHAR(20),
    @CreatedAt DATETIME2
AS
BEGIN
    SET NOCOUNT ON
    INSERT INTO Users (Id, Email, DisplayName, CreatedAt)
    VALUES (@Id, @Email, @DisplayName, @CreatedAt);
    SELECT * FROM Users WHERE Id = SCOPE_IDENTITY();
END;
GO

-- Get user details by Id
CREATE PROCEDURE sp_Users_GetById
    @Id VARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON
    SELECT * FROM Users WHERE Id = @Id;
END;
GO

-- Get all users for a specific board ordered by creation date
CREATE PROCEDURE sp_Users_GetByBoardId
    @BoardId BIGINT
AS
BEGIN
    SET NOCOUNT ON
    SELECT 
        u.Id
        ,u.Email
        ,u.DisplayName
        ,u.CreatedAt
    FROM Users u
    INNER JOIN BoardMembers bm ON u.Id = bm.UserId
    WHERE bm.BoardId = @BoardId
    ORDER BY u.CreatedAt DESC;
END;
GO

-- Get all users for a specific board with a role ordered by creation date
CREATE PROCEDURE sp_Users_GetByBoardIdWithRole
    @BoardId BIGINT
AS
BEGIN
    SET NOCOUNT ON
    SELECT 
        u.Id
        ,u.Email
        ,u.DisplayName
        ,u.CreatedAt
        ,bm.Role
    FROM Users u
    INNER JOIN BoardMembers bm ON u.Id = bm.UserId
    WHERE bm.BoardId = @BoardId
    ORDER BY u.CreatedAt DESC;
END;
GO

-- GetAll
CREATE PROCEDURE sp_Users_GetAll
AS
BEGIN
	SET NOCOUNT ON
    SELECT * FROM Users
    ORDER BY CreatedAt;
END;
GO

-- Any
CREATE PROCEDURE sp_Users_Any @Id VARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON
    SELECT CASE WHEN EXISTS (SELECT 1 FROM Users WHERE Id = @Id) THEN 1 ELSE 0 END;
END;
GO

-- Update user display name and email
CREATE PROCEDURE sp_Users_Update
    @Id VARCHAR(64),
    @Email NVARCHAR(255),
    @DisplayName NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON
    UPDATE Users 
    SET Email = @Email, DisplayName = @DisplayName 
    WHERE Id = @Id;
    SELECT * FROM Users WHERE Id = @Id;
END;
GO

-- Delete user
CREATE PROCEDURE sp_Users_Delete
    @Id VARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON
    DELETE FROM Users WHERE Id = @Id;
END;
GO

-- --------------------------------------------------------------------------------------------
-- Boards table stores projects or workspaces. Each board can have multiple columns and issues.
-- Create a new board (project)
CREATE PROCEDURE sp_Boards_Create
    @Title NVARCHAR(100),
    @Description NVARCHAR(500),
    @CreatedAt DATETIME2
AS
BEGIN
    SET NOCOUNT ON
    INSERT INTO Boards (Title, Description, CreatedAt)
    VALUES (@Title, @Description, @CreatedAt);
    SELECT * FROM Boards WHERE Id = SCOPE_IDENTITY();
END;
GO

-- Create a new board and assign the creating user as Admin, also creates default columns 
-- (To Do, In Progress, Done)
CREATE PROCEDURE sp_CreateBoardWithAdmin
    @Title NVARCHAR(100),
    @Description NVARCHAR(500),
    @UserId VARCHAR(64),
    @CreatedAt DATETIME2 
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. Insert a new record into the Boards table using the provided @CreatedAt
        INSERT INTO Boards (Title, Description, CreatedAt)
        VALUES (@Title, @Description, @CreatedAt);

        -- 2. Get the ID of the newly created Board
        DECLARE @NewBoardId BIGINT = SCOPE_IDENTITY();

        -- 3. Assign the user as the 'Admin' of the new board
        INSERT INTO BoardMembers (BoardId, UserId, Role)
        VALUES (@NewBoardId, @UserId, 'Admin');

        -- 4. Create default columns for the board
        -- Position 1: To Do
        -- Position 2: In Progress
        -- Position 3: Done
        INSERT INTO Columns (Name, Description, Position, BoardId)
        VALUES 
        (N'To Do', N'Tasks that are ready to be started', 1, @NewBoardId),
        (N'In Progress', N'Tasks that are currently being worked on', 2, @NewBoardId),
        (N'Done', N'Tasks that have been completed', 3, @NewBoardId);

        COMMIT TRANSACTION;

        -- Return the new board for further application use
        SELECT * FROM Boards WHERE Id = @NewBoardId;
    END TRY
    BEGIN CATCH
        -- Rollback the transaction if any error occurs
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Re-throw the error
        THROW;
    END CATCH
END;
GO

-- Get board details by Id
CREATE PROCEDURE sp_Boards_GetById
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON
    SELECT * FROM Boards WHERE Id = @Id;
END;
GO

-- Get all boards for a specific user ordered by creation date
CREATE PROCEDURE sp_Boards_GetByUserId
    @UserId VARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON
    SELECT 
        b.Id
        ,b.Title
        ,b.Description
        ,b.CreatedAt
    FROM Boards b
    INNER JOIN BoardMembers bm ON b.Id = bm.BoardId
    WHERE bm.UserId = @UserId
    ORDER BY b.CreatedAt DESC;
END;
GO

-- Get all boards in JSON-format for a specific user with his role ordered by creation date
CREATE PROCEDURE sp_Boards_GetByUserIdWithRoleJson
    @UserId VARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON
    SELECT 
        b.Id
        ,b.Title
        ,b.Description
        ,b.CreatedAt
        ,bm.Role -- Included so the app knows if user is Admin or User
    FROM Boards b
    INNER JOIN BoardMembers bm ON b.Id = bm.BoardId
    WHERE bm.UserId = @UserId
    ORDER BY b.CreatedAt DESC
    FOR JSON PATH;
END;
GO

-- GetAll boards (for admin dashboard, no user filter, ordered by creation date)
CREATE PROCEDURE sp_Boards_GetAll
AS
BEGIN
	SET NOCOUNT ON
    SELECT * FROM Boards
    ORDER BY CreatedAt;
END;
GO

-- Any
CREATE PROCEDURE sp_Boards_Any @Id VARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON
    SELECT CASE WHEN EXISTS (SELECT 1 FROM Boards WHERE Id = @Id) THEN 1 ELSE 0 END;
END;
GO

-- Update board title and description
CREATE PROCEDURE sp_Boards_Update
    @Id BIGINT,
    @Title NVARCHAR(100),
    @Description NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON
    UPDATE Boards 
    SET Title = @Title, Description = @Description 
    WHERE Id = @Id;
    SELECT * FROM Boards WHERE Id = @Id;
END;
GO

-- Delete board (cascades to BoardMembers and Columns)
CREATE PROCEDURE sp_Boards_Delete
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON
    DELETE FROM Boards WHERE Id = @Id;
END;
GO

-- --------------------------------------------------------------------------------------------
-- BoardMembers table manages user roles (Admin vs User) for each board. A user can have different 
-- roles in different boards.
-- Add a user to a board with a specific role
CREATE PROCEDURE sp_BoardMembers_Create
    @BoardId BIGINT,
    @UserId VARCHAR(64),
    @Role NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON
    INSERT INTO BoardMembers (BoardId, UserId, Role)
    VALUES (@BoardId, @UserId, @Role);
END;
GO

-- Get all members of a specific board
CREATE PROCEDURE sp_BoardMembers_GetByBoardId
    @BoardId BIGINT
AS
BEGIN
    SET NOCOUNT ON
    SELECT UserId, Role FROM BoardMembers WHERE BoardId = @BoardId;
END;
GO

-- Change user role within a board
CREATE PROCEDURE sp_BoardMembers_UpdateRole
    @BoardId BIGINT,
    @UserId VARCHAR(64),
    @Role NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON
    UPDATE BoardMembers SET Role = @Role 
    WHERE BoardId = @BoardId AND UserId = @UserId;
END;
GO

-- Remove a user from a board
CREATE PROCEDURE sp_BoardMembers_Delete
    @BoardId BIGINT,
    @UserId VARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON
    DELETE FROM BoardMembers WHERE BoardId = @BoardId AND UserId = @UserId;
END;
GO

-- --------------------------------------------------------------------------------------------
-- Create a column at the end of the board (calculates position based on existing columns)
CREATE PROCEDURE sp_Columns_Create
    @Name NVARCHAR(50),
    @Description NVARCHAR(200),
    @BoardId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    -- Calculate the next position for the new column starting from 0
    DECLARE @NextPosition INT;
    
    -- If no columns exist, COALESCE/ISNULL will return -1, so +1 results in 0
    -- Alternatively: if exists returns MAX + 1, if not returns 0
    SELECT @NextPosition = CASE 
        WHEN EXISTS (SELECT 1 FROM Columns WHERE BoardId = @BoardId) 
        THEN (SELECT MAX(Position) + 1 FROM Columns WHERE BoardId = @BoardId)
        ELSE 0 
    END;

    -- Insert the new column
    INSERT INTO Columns (Name, Description, Position, BoardId)
    VALUES (@Name, @Description, @NextPosition, @BoardId);

    -- Return the newly created record
    -- Using the specific ID to ensure performance and accuracy
    SELECT * FROM Columns WHERE Id = SCOPE_IDENTITY();
END;
GO

-- Get column detailes by id
CREATE PROCEDURE sp_Columns_GetById
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON
    SELECT *  FROM Columns WHERE Id = @Id;
END;
GO

-- Get all columns for a specific board ordered by position
CREATE PROCEDURE sp_Columns_GetByBoardId
    @BoardId BIGINT
AS
BEGIN
    SET NOCOUNT ON
    SELECT * FROM Columns 
    WHERE BoardId = @BoardId ORDER BY Position;
END;
GO

-- GetAll
CREATE PROCEDURE sp_Columns_GetAll
AS
BEGIN
	SET NOCOUNT ON
    SELECT * FROM Columns;
END;
GO

CREATE PROCEDURE sp_Columns_Any @Id BIGINT
AS
BEGIN
	SET NOCOUNT ON
    SELECT CASE WHEN EXISTS (SELECT 1 FROM Columns WHERE Id = @Id) THEN 1 ELSE 0 END;
END;
GO

-- Update column details and position
CREATE PROCEDURE sp_Columns_Update
    @Id BIGINT,
    @Name NVARCHAR(50),
    @Description NVARCHAR(200),
    @Position INT
AS
BEGIN
    SET NOCOUNT ON
    UPDATE Columns 
    SET Name = @Name, Description = @Description, Position = @Position 
    WHERE Id = @Id;
    SELECT * FROM Columns WHERE Id = @Id;
END;
GO

-- Delete a column
CREATE PROCEDURE sp_Columns_Delete
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON
    DELETE FROM Columns WHERE Id = @Id;
END;
GO

-- --------------------------------------------------------------------------------------------
-- Issues table represents the tasks or cards within a column. Each issue has a title, description,
-- due date, and is linked to a specific column. Issues can be created by one user and assigned 
-- to another.
-- Create a new issue (task)
CREATE PROCEDURE sp_Issues_Create
    @Title NVARCHAR(200),
    @Description NVARCHAR(2000),
    @DueDate DATETIME2,
    @PositionInColumn BIGINT,
    @ColumnId BIGINT,
    @CreatorId VARCHAR(64),
    @AssigneeId VARCHAR(64),
    @CreatedAt DATETIME2
AS
BEGIN
    SET NOCOUNT ON
    INSERT INTO Issues (Title, Description, DueDate, CreatedAt, PositionInColumn, ColumnId, CreatorId, AssigneeId)
    VALUES (@Title, @Description, @DueDate, @CreatedAt, @PositionInColumn, @ColumnId, @CreatorId, @AssigneeId);
    SELECT * FROM Issues WHERE Id = SCOPE_IDENTITY();
END;
GO

-- Get issue detailes by id
CREATE PROCEDURE sp_Issues_GetById
    @Id BIGINT
AS
BEGIN
	SET NOCOUNT ON
    SELECT * FROM Issues WHERE Id = @Id;
END
GO

-- Get all issues in a specific column ordered by position
CREATE PROCEDURE sp_Issues_GetByColumnId
    @ColumnId BIGINT
AS
BEGIN
    SET NOCOUNT ON
    SELECT * FROM Issues WHERE ColumnId = @ColumnId ORDER BY PositionInColumn;
END;
GO

-- Get all issues for a specific board
CREATE PROCEDURE sp_Issues_GetByBoardIdJson
    @BoardId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the board exists before processing
    IF NOT EXISTS (SELECT 1 FROM Boards WHERE Id = @BoardId)
    BEGIN
        SELECT NULL AS BoardJson;
        RETURN;
    END

    -- Constructing the hierarchy using Nested FOR JSON PATH
    SELECT 
        b.Id, 
        b.Title, 
        b.Description, 
        b.CreatedAt,
        (
            -- Subquery for Columns
            SELECT 
                c.Id, 
                c.Name, 
                c.Description, 
                c.Position,
                (
                    -- Subquery for Issues within each Column
                    SELECT 
                        i.Id, 
                        i.Title, 
                        i.Description, 
                        i.DueDate, 
                        i.CreatedAt, 
                        i.PositionInColumn,
                        i.ColumnId,
                        i.CreatorId, 
                        i.AssigneeId,
                        u2.DisplayName AS CreatorName,
                        u3.DisplayName AS AssigneeName
                    FROM Issues i
                    INNER JOIN Users u2 ON i.CreatorId = u2.Id
                    LEFT JOIN Users u3 ON i.AssigneeId = u3.Id
                    WHERE i.ColumnId = c.Id
                    ORDER BY i.PositionInColumn
                    FOR JSON PATH
                ) AS Issues
            FROM Columns c
            WHERE c.BoardId = b.Id
            ORDER BY c.Position
            FOR JSON PATH
        ) AS Columns
    FROM Boards b
    WHERE b.Id = @BoardId
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;
END;
GO

-- GetAll
CREATE PROCEDURE sp_Issues_GetAll
AS
BEGIN
	SET NOCOUNT ON
    SELECT * FROM Issues ORDER BY ColumnId, PositionInColumn;
END;
GO

-- Any
CREATE PROCEDURE sp_Issues_Any @Id BIGINT
AS
BEGIN
	SET NOCOUNT ON
    SELECT CASE WHEN EXISTS (SELECT 1 FROM Issues WHERE Id = @Id) THEN 1 ELSE 0 END;
END;
GO


-- Update issue details, assignee, or move to another column
CREATE PROCEDURE sp_Issues_Update
    @Id BIGINT,
    @Title NVARCHAR(200),
    @Description NVARCHAR(2000),
    @DueDate DATETIME2,
    @AssigneeId VARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON
    UPDATE Issues
    SET Title = @Title, 
        Description = @Description, 
        DueDate = @DueDate, 
        AssigneeId = @AssigneeId
    WHERE Id = @Id;
    SELECT * FROM Issues WHERE Id = @Id;
END;
GO

-- Delete an issue
CREATE PROCEDURE sp_Issues_Delete
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON
    DELETE FROM Issues WHERE Id = @Id;
END;
GO

-- --------------------------------------------------------------------------------------------