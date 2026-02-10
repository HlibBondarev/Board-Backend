USE BoardDb;
GO

-- Post
CREATE PROCEDURE dbo.User_Post
    @Email NVARCHAR(255),
    @DisplayName NVARCHAR(20),
	@CreatedAt DATETIME2
AS
BEGIN
    INSERT INTO Users (Email, DisplayName, CreatedAt)
    VALUES (@Email, @DisplayName, @CreatedAt);

    SELECT * FROM Users WHERE Id = SCOPE_IDENTITY();
END
GO

-- GetSingle
CREATE PROCEDURE dbo.User_GetSingle
    @Id INT
AS
BEGIN
    SELECT * FROM Users WHERE Id = @Id;
END
GO

-- GetAll
CREATE PROCEDURE dbo.User_GetAll
AS
BEGIN
    SELECT Id, Email, DisplayName, CreatedAt FROM Users;
END;
GO

-- Any
CREATE PROCEDURE dbo.User_Any @Id INT
AS
BEGIN
    SELECT CASE WHEN EXISTS (SELECT 1 FROM Users WHERE Id = @Id) THEN 1 ELSE 0 END;
END;
GO

-- Put (Update)
CREATE PROCEDURE dbo.User_Put
    @Id INT,
    @Email NVARCHAR(255),
    @DisplayName NVARCHAR(20),
	@CreatedAt DATETIME2
AS
BEGIN
    UPDATE Users 
    SET Email = @Email, DisplayName = @DisplayName, CreatedAt = @CreatedAt
    WHERE Id = @Id;
    SELECT * FROM Users WHERE Id = @Id;
END;
GO

-- Delete
CREATE PROCEDURE dbo.User_Delete @Id INT
AS
BEGIN
    DELETE FROM Users WHERE Id = @Id;
END;
GO

-- Column ---------------------------------------------------------------------------------
-- Post
CREATE PROCEDURE dbo.Column_Post
    @Name NVARCHAR(50),
	@Description NVARCHAR(200),
    @Position INT,
	@UserId  INT
AS
BEGIN
    INSERT INTO Columns (Name, Description, Position, UserId)
    VALUES (@Name, @Description, @Position, @UserId);

    SELECT * FROM Columns WHERE Id = SCOPE_IDENTITY();
END
GO

-- GetSingle
CREATE PROCEDURE dbo.Column_GetSingle
    @Id INT
AS
BEGIN
    SELECT * FROM Columns WHERE Id = @Id;
END
GO

-- GetAll
CREATE PROCEDURE dbo.Column_GetAll
AS
BEGIN
    SELECT Id, Name, Description, Position, UserId FROM Columns;
END;
GO

-- Any
CREATE PROCEDURE dbo.Column_Any @Id INT
AS
BEGIN
    SELECT CASE WHEN EXISTS (SELECT 1 FROM Columns WHERE Id = @Id) THEN 1 ELSE 0 END;
END;
GO

-- Put (Update)
CREATE PROCEDURE dbo.Column_Put
    @Id INT,
    @Name NVARCHAR(50),
	@Description NVARCHAR(200),
    @Position INT,
	@UserId INT
AS
BEGIN
    UPDATE Columns SET Name = @Name, Description = @Description, Position = @Position, UserId = @UserId WHERE Id = @Id;
    SELECT * FROM Columns WHERE Id = @Id;
END;
GO

-- Delete
CREATE PROCEDURE dbo.Column_Delete @Id INT
AS
BEGIN
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
	@PositionInColumn INT,
    @ColumnId INT,
    @CreatorId INT,
    @AssigneeId INT
AS
BEGIN
    INSERT INTO Issues (Title, Description, DueDate, CreatedAt, PositionInColumn, ColumnId, CreatorId, AssigneeId)
    VALUES (@Title, @Description, @DueDate, @CreatedAt, @PositionInColumn, @ColumnId, @CreatorId, @AssigneeId);

    SELECT * FROM Issues WHERE Id = SCOPE_IDENTITY();
END
GO

-- GetSingle
CREATE PROCEDURE dbo.Issue_GetSingle
    @Id INT
AS
BEGIN
    SELECT * FROM Issues WHERE Id = @Id;
END
GO

-- GetAll
CREATE PROCEDURE dbo.Issue_GetAll
AS
BEGIN
    SELECT Id, Title, Description, DueDate, CreatedAt, PositionInColumn, ColumnId, CreatorId, AssigneeId FROM Issues;
END;
GO

-- Any
CREATE PROCEDURE dbo.Issue_Any @Id INT
AS
BEGIN
    SELECT CASE WHEN EXISTS (SELECT 1 FROM Issues WHERE Id = @Id) THEN 1 ELSE 0 END;
END;
GO

-- Put (Update)
CREATE PROCEDURE dbo.Issue_Put
    @Id INT,
    @Title NVARCHAR(200),
    @Description NVARCHAR(2000),
    @DueDate DATETIME2,
    @CreatedAt DATETIME2,
	@PositionInColumn INT,
    @ColumnId INT,
    @CreatorId INT,
    @AssigneeId INT
AS
BEGIN
    UPDATE Issues 
    SET Title = @Title, Description = @Description, DueDate = @DueDate,
		CreatedAt = @CreatedAt, PositionInColumn = @PositionInColumn,
        ColumnId = @ColumnId, CreatorId = @CreatorId, AssigneeId = @AssigneeId
    WHERE Id = @Id;
    SELECT * FROM Issues WHERE Id = @Id;
END;
GO

-- Delete
CREATE PROCEDURE dbo.Issue_Delete @Id INT
AS
BEGIN
    DELETE FROM Issues WHERE Id = @Id;
END;
GO