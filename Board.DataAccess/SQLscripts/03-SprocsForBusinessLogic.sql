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

-- Adds the User as a member or updates their role if they are already a member
CREATE PROCEDURE sp_Boards_AddMember
    @BoardId BIGINT,
    @Email NVARCHAR(255),
    @Role NVARCHAR(20)
AS
BEGIN
    -- Set NOCOUNT ON to prevent extra result sets
    SET NOCOUNT ON;

    -- Validate if the provided role is allowed
    IF @Role NOT IN ('Admin', 'User')
    BEGIN
        RAISERROR('Invalid role. Allowed values are "Admin" or "User".', 16, 1);
        RETURN;
    END

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

    BEGIN TRY
        -- Check if the user is already a member of the board
        IF EXISTS (SELECT 1 FROM BoardMembers WHERE BoardId = @BoardId AND UserId = @UserId)
        BEGIN
            -- Update existing member's role
            UPDATE BoardMembers
            SET Role = @Role
            WHERE BoardId = @BoardId AND UserId = @UserId;
        END
        ELSE
        BEGIN
            -- Insert the new board member
            INSERT INTO BoardMembers (BoardId, UserId, Role)
            VALUES (@BoardId, @UserId, @Role);
        END
    END TRY
    BEGIN CATCH
        -- Handle potential errors during execution
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
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