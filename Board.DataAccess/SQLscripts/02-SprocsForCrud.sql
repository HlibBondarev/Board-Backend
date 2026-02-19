USE BoardDb;
GO

-- Post
CREATE PROCEDURE dbo.User_Post
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
END
GO

-- GetSingle
CREATE PROCEDURE dbo.User_GetSingle
    @Id VARCHAR(64)
AS
BEGIN
	SET NOCOUNT ON

    SELECT * FROM Users WHERE Id = @Id;
END
GO

-- GetAll
CREATE PROCEDURE dbo.User_GetAll
AS
BEGIN
	SET NOCOUNT ON

    SELECT Id, Email, DisplayName, CreatedAt FROM Users;
END;
GO

-- Any
CREATE PROCEDURE dbo.User_Any @Id VARCHAR(64)
AS
BEGIN
    SELECT CASE WHEN EXISTS (SELECT 1 FROM Users WHERE Id = @Id) THEN 1 ELSE 0 END;
END;
GO

-- Put (Update)
CREATE PROCEDURE dbo.User_Put
    @Id VARCHAR(64),
    @Email NVARCHAR(255),
    @DisplayName NVARCHAR(20),
	@CreatedAt DATETIME2
AS
BEGIN
	SET NOCOUNT ON

    UPDATE Users 
    SET Email = @Email, DisplayName = @DisplayName, CreatedAt = @CreatedAt
    WHERE Id = @Id;
    SELECT * FROM Users WHERE Id = @Id;
END;
GO

-- Delete
CREATE PROCEDURE dbo.User_Delete @Id VARCHAR(64)
AS
BEGIN
	SET NOCOUNT ON

    DELETE FROM Users WHERE Id = @Id;
END;
GO

-- Column ---------------------------------------------------------------------------------
-- Post
CREATE PROCEDURE dbo.Column_Post
    @Name NVARCHAR(50),
	@Description NVARCHAR(200),
    @Position INT,
	@UserId  VARCHAR(64)
AS
BEGIN
	SET NOCOUNT ON

    INSERT INTO Columns (Name, Description, Position, UserId)
    VALUES (@Name, @Description, @Position, @UserId);

    SELECT * FROM Columns WHERE Id = SCOPE_IDENTITY();
END
GO

-- GetSingle
CREATE PROCEDURE dbo.Column_GetSingle
    @Id BIGINT
AS
BEGIN
	SET NOCOUNT ON

    SELECT * FROM Columns WHERE Id = @Id;
END
GO

-- GetAll
CREATE PROCEDURE dbo.Column_GetAll
AS
BEGIN
	SET NOCOUNT ON

    SELECT Id, Name, Description, Position, UserId FROM Columns ORDER BY Position;
END;
GO

-- Get all columns belonging to a specific User
CREATE PROCEDURE dbo.Column_GetColumnsByUser
    @UserId VARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON; -- Prevents sending extra "rows affected" messages for speed

    SELECT Id, Name, Description, Position, UserId
    FROM Columns
    WHERE UserId = @UserId
    ORDER BY Position;
END;
GO

-- Any
CREATE PROCEDURE dbo.Column_Any @Id BIGINT
AS
BEGIN
	SET NOCOUNT ON

    SELECT CASE WHEN EXISTS (SELECT 1 FROM Columns WHERE Id = @Id) THEN 1 ELSE 0 END;
END;
GO

-- Put (Update)
CREATE PROCEDURE dbo.Column_Put
    @Id BIGINT,
    @Name NVARCHAR(50),
	@Description NVARCHAR(200),
    @Position INT,
	@UserId VARCHAR(64)
AS
BEGIN
	SET NOCOUNT ON

    UPDATE Columns SET Name = @Name, Description = @Description, Position = @Position, UserId = @UserId WHERE Id = @Id;
    SELECT * FROM Columns WHERE Id = @Id;
END;
GO

-- Delete
CREATE PROCEDURE dbo.Column_Delete @Id BIGINT
AS
BEGIN
	SET NOCOUNT ON

    DELETE FROM Columns WHERE Id = @Id;
END;
GO

-- Issue ---------------------------------------------------------------------------------
-- Post
CREATE PROCEDURE dbo.Issue_Post
    @Title NVARCHAR(200),
    @Description NVARCHAR(2000),
    @DueDate DATETIME2,
    @CreatedAt DATETIME2,
	@PositionInColumn BIGINT,
    @ColumnId BIGINT,
    @CreatorId VARCHAR(64),
    @AssigneeId VARCHAR(64)
AS
BEGIN
	SET NOCOUNT ON

    INSERT INTO Issues (Title, Description, DueDate, CreatedAt, PositionInColumn, ColumnId, CreatorId, AssigneeId)
    VALUES (@Title, @Description, @DueDate, @CreatedAt, @PositionInColumn, @ColumnId, @CreatorId, @AssigneeId);

    SELECT * FROM Issues WHERE Id = SCOPE_IDENTITY();
END
GO

-- GetSingle
CREATE PROCEDURE dbo.Issue_GetSingle
    @Id BIGINT
AS
BEGIN
	SET NOCOUNT ON

    SELECT * FROM Issues WHERE Id = @Id;
END
GO

-- GetAll
CREATE PROCEDURE dbo.Issue_GetAll
AS
BEGIN
	SET NOCOUNT ON

    SELECT Id, Title, Description, DueDate, CreatedAt, PositionInColumn, ColumnId, CreatorId, AssigneeId 
    FROM Issues ORDER BY ColumnId, PositionInColumn;
END;
GO

-- Get all issues for all columns belonging to a specific User
CREATE PROCEDURE dbo.Issue_GetIssuesByColumnsForUsers
    @UserId VARCHAR(64)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT i.Id, i.Title, i.Description, i.DueDate, i.CreatedAt, 
           i.PositionInColumn, i.ColumnId, i.CreatorId, i.AssigneeId
    FROM Issues i
    INNER JOIN Columns c ON i.ColumnId = c.Id
    WHERE c.UserId = @UserId
    ORDER BY i.ColumnId, i.PositionInColumn;
END;
GO

-- Any
CREATE PROCEDURE dbo.Issue_Any @Id BIGINT
AS
BEGIN
	SET NOCOUNT ON

    SELECT CASE WHEN EXISTS (SELECT 1 FROM Issues WHERE Id = @Id) THEN 1 ELSE 0 END;
END;
GO

-- Put (Update)
CREATE PROCEDURE dbo.Issue_Put
    @Id BIGINT,
    @Title NVARCHAR(200),
    @Description NVARCHAR(2000),
    @DueDate DATETIME2,
    @AssigneeId VARCHAR(64)
AS
BEGIN
	SET NOCOUNT ON

    UPDATE Issues 
    SET Title = @Title, Description = @Description, DueDate = @DueDate, AssigneeId = @AssigneeId
    WHERE Id = @Id;
    SELECT * FROM Issues WHERE Id = @Id;
END;
GO

-- Delete
CREATE PROCEDURE dbo.Issue_Delete @Id BIGINT
AS
BEGIN
	SET NOCOUNT ON

    DELETE FROM Issues WHERE Id = @Id;
END;
GO