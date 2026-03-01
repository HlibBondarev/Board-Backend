-- Issue ---------------------------------------------------------------------------------
-- Move an issue to a different position within the same column or to a different column, 
-- ensuring that the order of issues is maintained correctly in both source and target columns.
CREATE PROCEDURE sp_Issues_Move
    @IssueId BIGINT,
    @TargetColumnId BIGINT,
    @NewPosition INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @OldColumnId BIGINT, @OldPosition INT;

        -- Get current state of the issue
        SELECT @OldColumnId = ColumnId, @OldPosition = PositionInColumn 
        FROM Issues WHERE Id = @IssueId;

        -- Exit if issue does not exist
        IF @OldColumnId IS NULL
        BEGIN
            ROLLBACK TRANSACTION;
            RETURN;
        END

        IF @OldColumnId = @TargetColumnId
        BEGIN
            -- CASE 1: Internal move within the same column
            -- Updates only the affected range between positions
            IF @OldPosition < @NewPosition
            BEGIN
                -- Moving forward (down): shift intermediate items back (up)
                UPDATE Issues 
                SET PositionInColumn = PositionInColumn - 1
                WHERE ColumnId = @OldColumnId 
                  AND PositionInColumn > @OldPosition 
                  AND PositionInColumn <= @NewPosition;
            END
            ELSE IF @OldPosition > @NewPosition
            BEGIN
                -- Moving backward (up): shift intermediate items forward (down)
                UPDATE Issues 
                SET PositionInColumn = PositionInColumn + 1
                WHERE ColumnId = @OldColumnId 
                  AND PositionInColumn >= @NewPosition 
                  AND PositionInColumn < @OldPosition;
            END
        END
        ELSE
        BEGIN
            -- CASE 2: Moving between different columns
            -- 1. Close the gap in the source column
            UPDATE Issues 
            SET PositionInColumn = PositionInColumn - 1
            WHERE ColumnId = @OldColumnId 
              AND PositionInColumn > @OldPosition;

            -- 2. Create space in the target column
            UPDATE Issues 
            SET PositionInColumn = PositionInColumn + 1
            WHERE ColumnId = @TargetColumnId 
              AND PositionInColumn >= @NewPosition;
        END

        -- Final Step: Place the moving issue into the exact target spot
        UPDATE Issues
        SET ColumnId = @TargetColumnId, 
            PositionInColumn = @NewPosition
        WHERE Id = @IssueId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Reorder issues in Column with id = ColumnId
CREATE PROCEDURE sp_Issues_ReorderInColumn
    @IssuePosition INT,
    @ColumnId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        BEGIN
        -- Moving forward (down): shift intermediate items back (up)
        UPDATE Issues
        SET PositionInColumn = PositionInColumn - 1
        WHERE ColumnId = @ColumnId 
        AND PositionInColumn > @IssuePosition;
        END
    COMMIT TRANSACTION;
    END TRY
    
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Reorder columns in Board with id = BoardId
CREATE PROCEDURE sp_Columns_ReorderInBoard
    @ColumnPosition INT,
    @BoardId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;
    BEGIN TRY
        BEGIN
        -- Moving forward (down): shift intermediate items back (up)
        UPDATE Columns
        SET Position = Position - 1
        WHERE BoardId = @BoardId
        AND Position > @ColumnPosition;
        END
    COMMIT TRANSACTION;
    END TRY
    
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Get all issues in Column with id = ColumnId
CREATE PROCEDURE sp_Issues_GetByColumnIdWithUsersJson
    @ColumnId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    -- Використовуємо SELECT для вибору полів, що відповідають вашому DTO
    -- FOR JSON PATH автоматично серіалізує результат у JSON рядок
    SELECT 
        i.Id,
        i.Title,
        i.Description,
        i.DueDate,
        i.CreatedAt,
        i.PositionInColumn,
        i.ColumnId,
        i.CreatorId,
        u_creator.DisplayName AS CreatorName,
        i.AssigneeId,
        u_assignee.DisplayName AS AssigneeName
    FROM Issues i
    INNER JOIN Users u_creator ON i.CreatorId = u_creator.Id
    LEFT JOIN Users u_assignee ON i.AssigneeId = u_assignee.Id
    WHERE i.ColumnId = @ColumnId
    ORDER BY i.PositionInColumn
    FOR JSON PATH;
END
GO

-- Checks if the User is already a member of the Board with a specific BoardId 
CREATE PROCEDURE sp_Boards_CheckMemberExistence
    @BoardId BIGINT,
    @Email NVARCHAR(255)
AS
BEGIN
    -- Set NOCOUNT ON to prevent extra result sets
    SET NOCOUNT ON;

    -- Check for existence and return 1 (true) or 0 (false)
    IF EXISTS (
        SELECT 1 
        FROM BoardMembers bm
        JOIN Users u ON bm.UserId = u.Id
        WHERE bm.BoardId = @BoardId AND u.Email = @Email
    )
    BEGIN
        SELECT CAST(1 AS BIT) AS IsMember;
    END
    ELSE
    BEGIN
        SELECT CAST(0 AS BIT) AS IsMember;
    END
END;
GO

-- Adds the User as a member of the Board with a specific BoardId
CREATE PROCEDURE sp_Boards_AddMember
    @BoardId BIGINT,
    @Email NVARCHAR(255),
    @Role NVARCHAR(20)
AS
BEGIN
    -- Set NOCOUNT ON to prevent extra result sets from interfering with SELECT statements.
    SET NOCOUNT ON;

    DECLARE @UserId VARCHAR(64);

    -- Find the UserId associated with the provided email
    SELECT @UserId = Id 
    FROM Users 
    WHERE Email = @Email;

    -- Check if the user exists
    IF @UserId IS NULL
    BEGIN
        RAISERROR('User with the specified email does not exist.', 16, 1);
        RETURN;
    END

    -- Check if the board exists
    IF NOT EXISTS (SELECT 1 FROM Boards WHERE Id = @BoardId)
    BEGIN
        RAISERROR('Board with the specified ID does not exist.', 16, 1);
        RETURN;
    END

    -- Check if the user is already a member of the board
    IF EXISTS (SELECT 1 FROM BoardMembers WHERE BoardId = @BoardId AND UserId = @UserId)
    BEGIN
        RAISERROR('User is already a member of this board.', 16, 1);
        RETURN;
    END

    -- Insert the new board member
    BEGIN TRY
        INSERT INTO BoardMembers (BoardId, UserId, Role)
        VALUES (@BoardId, @UserId, @Role);
    END TRY
    BEGIN CATCH
        -- Handle potential errors during insertion (e.g., Role check constraint)
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO

-- Remove the User as a member of the Board with a specific BoardId
CREATE PROCEDURE sp_Boards_RemoveMember
    @BoardId BIGINT,
    @Email NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @UserId VARCHAR(64);
        DECLARE @UserRole NVARCHAR(20);

        -- Find the user ID and their role on the specific board
        SELECT @UserId = u.Id, @UserRole = bm.Role
        FROM Users u
        JOIN BoardMembers bm ON u.Id = bm.UserId
        WHERE u.Email = @Email AND bm.BoardId = @BoardId;

        -- If user is not found on this board, exit the procedure
        IF @UserId IS NULL
        BEGIN
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Business rule: prevent deleting the last Admin
        IF @UserRole = 'Admin'
        BEGIN
            DECLARE @AdminCount INT;
            
            SELECT @AdminCount = COUNT(*)
            FROM BoardMembers
            WHERE BoardId = @BoardId AND Role = 'Admin';

            IF @AdminCount <= 1
            BEGIN
                RAISERROR('Cannot remove the last Admin from the board.', 16, 1);
            END
        END

        -- Delete the member record
        DELETE FROM BoardMembers
        WHERE BoardId = @BoardId AND UserId = @UserId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Rollback transaction if active
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Re-throw the error to the calling application
        THROW;
    END CATCH
END;
GO