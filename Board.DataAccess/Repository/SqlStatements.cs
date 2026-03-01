namespace Board.DataAccess.Repository;

public static class SqlStatements
{
    public static class ForUsers
    {
        public const string Create =
            @"EXEC sp_Users_Create 
            @Id = @Id, @Email = @Email, 
            @DisplayName = @DisplayName, 
            @CreatedAt = @CreatedAt";
        public const string GetById =
            @"EXEC sp_Users_GetById 
            @Id = @Id";
        public const string GetByBoardId =
            @"EXEC sp_Users_GetByBoardId 
            @BoardId = @BoardId";
        public const string GetByBoardIdWithRole =
            @"EXEC sp_Users_GetByBoardId 
            @BoardId = @BoardId";
        public const string GetAll =
            @"EXEC sp_Users_GetAll";
        public const string Any =
            @"EXEC sp_Users_Any";
        public const string Update =
            @"EXEC sp_Users_Update 
            @Id = @Id, @Email = @Email, 
            @DisplayName = @DisplayName";
        public const string Delete =
            @"EXEC sp_Users_Delete @Id = @Id";
        public const string UserEmailIsExists =
           @"sp_Users_EmailIsExists";
    }

    public static class ForBoards
    {
        public const string Create =
            @"EXEC sp_Boards_Create 
            @Id = @Id, @Title = @Title, 
            @Description = @Description, 
            @CreatedAt = @CreatedAt";
        public const string CreateWithAdmin =
            @"sp_CreateBoardWithAdmin";
        public const string GetById =
            @"EXEC sp_Boards_GetById 
            @Id = @Id";
        public const string GetBoardsByUserId =
            @"EXEC sp_Boards_GetByUserId 
            @UserId = @UserId";
        public const string GetBoardsByUserIdWithRole =
            @"sp_Boards_GetByUserIdWithRoleJson";
        public const string GetAll =
            @"EXEC sp_Boards_GetAll";
        public const string Any =
            @"EXEC sp_Boards_Any";
        public const string Update =
            @"EXEC sp_Boards_Update 
            @Id = @Id, @Title = @Title, 
            @Description = @Description";
        public const string Delete =
            @"EXEC sp_Boards_Delete @Id = @Id";
        public const string CheckBoardMemberExistence =
           @"sp_Boards_CheckMemberExistence";
        public const string AddBoardMember =
           @"sp_Boards_AddMember";
    }

    public static class ForColumns
    {
        public const string Create =
            @"EXEC sp_Columns_Create 
            @Name = @Name, @Description = @Description, 
            @BoardId = @BoardId";
        public const string GetById =
            @"EXEC sp_Columns_GetById 
            @Id = @Id";
        public const string GetByBoardId =
            @"EXEC sp_Columns_GetByBoardId 
            @BoardId = @BoardId";
        public const string GetAll =
            @"EXEC sp_Columns_GetAll";
        public const string Any =
            @"EXEC sp_Columns_Any";
        public const string Update =
            @"EXEC sp_Columns_Update 
            @Id = @Id, @Name = @Name, 
            @Description = @Description, 
            @Position = @Position";
        public const string Delete =
            @"EXEC sp_Columns_Delete @Id = @Id";
        public const string ReorderColumnsInBoard =
            @"EXEC sp_Columns_ReorderInBoard
            @ColumnPosition = @ColumnPosition, 
            @BoardId = @BoardId";
    }

    public static class ForIssues
    {
        public const string Create =
            @"EXEC sp_Issues_Create 
            @Title = @Title, @Description = @Description,
            @DueDate = @DueDate, @CreatedAt = @CreatedAt,
            @PositionInColumn = @PositionInColumn, @ColumnId = @ColumnId,
            @CreatorId = @CreatorId, @AssigneeId = @AssigneeId";
        public const string GetById =
            @"EXEC sp_Issues_GetById 
            @Id = @Id";
        public const string GetByColumnId =
            @"EXEC sp_Issues_GetByColumnId 
            @ColumnId = @ColumnId";
        public const string GetByBoardId =
            @"sp_Issues_GetByBoardIdJson";
        public const string GetByColumnIdWithUsers =
            @"sp_Issues_GetByColumnIdWithUsersJson";
        public const string GetAll =
            @"EXEC sp_Issues_GetAll";
        public const string Any =
            @"EXEC sp_Issues_Any";
        public const string Update = @"EXEC sp_Issues_Update
            @Id = @Id, @Title = @Title, @DueDate = @DueDate,
            @Description = @Description, @AssigneeId = @AssigneeId";
        public const string Delete =
            @"EXEC sp_Issues_Delete 
            @Id = @Id";
        public const string MoveIssue =
            @"EXEC sp_Issues_Move
            @IssueId = @IssueId, @TargetColumnId = @TargetColumnId,
            @NewPosition = @NewPosition";
        public const string ReorderIssuesInColumn =
            @"EXEC sp_Issues_ReorderInColumn
            @IssuePosition = @IssuePosition, 
            @ColumnId = @ColumnId";
    }
}