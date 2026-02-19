-- Issue ---------------------------------------------------------------------------------
-- Move an issue to a different position within the same column or to a different column, 
-- ensuring that the order of issues is maintained correctly in both source and target columns.
CREATE PROCEDURE dbo.Issue_Move
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

-- Get all issues belonging to a specific Column
CREATE PROCEDURE dbo.Issue_GetIssuesByColumn
    @ColumnId BIGINT
AS
BEGIN
	SET NOCOUNT ON

    SELECT * FROM Issues WHERE ColumnId = @ColumnId;
END
GO

-- Re order issues in a column after an issue has been removed or moved out of the column, 
-- ensuring that the PositionInColumn values are contiguous and correctly reflect the new 
-- order of issues within the column.

CREATE PROCEDURE dbo.Issue_ReorderInColumn
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