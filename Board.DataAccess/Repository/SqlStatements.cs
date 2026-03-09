namespace Board.DataAccess.Repository;

public static class SqlStatements
{
    public static class ForUsers
    {
        public const string Create =
            @"EXEC sp_Users_Create
            @Id, @Email, @DisplayName, @CreatedAt";
        public const string Update =
            @"EXEC sp_Users_Update 
            @Id, @Email, @DisplayName";
        public const string GetById =
            "sp_Users_GetById";
        public const string GetByEmail =
            "sp_Users_GetByEmail";
        public const string GetAll =
            "sp_Users_GetAll";
        public const string Exists =
            "sp_Users_Exists";
        public const string Delete =
            "sp_Users_Delete";
    }

    public static class ForBoards
    {
        public const string Create =
            @"EXEC sp_Boards_Create
            @Title, @Description, @CreatedAt";
        public const string CreateWithAdmin =
            @"EXEC sp_Boards_CreateWithAdmin
            @Title, @Description, @CreatedAt, @UserId";
        public const string Update =
            @"EXEC sp_Boards_Update
            @Id, @Title, @Description";
        public const string GetById =
            "sp_Boards_GetById";
        public const string GetBoardsByUserIdWithRoleInJson =
            "sp_Boards_GetByUserIdWithRoleJson";
        public const string GetAll =
            "sp_Boards_GetAll";
        public const string Exists =
            "sp_Boards_Exists";
        public const string Delete =
            "sp_Boards_Delete";
        public const string CheckBoardMembershipByUserId =
            "sp_Boards_CheckMembershipByUserId";
        public const string CheckBoardMembershipByEmail =
            "sp_Boards_CheckMembershipByEmail";
        public const string CheckBoardMembershipWithRoleByEmail =
            "sp_Boards_CheckMembershipWithRoleByEmail";
        public const string CheckUserIsBoardAdmin =
            "sp_Boards_CheckUserIsAdmin";
        // Storage procedures for Business Logic
        public const string AddBoardMember =
           "sp_Boards_AddMember";
        public const string RemoveBoardMember =
           "sp_Boards_RemoveMember";
    }

    public static class ForColumns
    {
        public const string Create =
            @"EXEC sp_Columns_Create
            @Name, @Description, @BoardId";
        public const string Update =
            @"EXEC sp_Columns_Update
            @Id, @Name, @Description, @Position";
        public const string GetById =
            "sp_Columns_GetById";
        public const string GetAll =
            "sp_Columns_GetAll";
        public const string Exists =
            "sp_Columns_Exists";
        public const string Delete =
            "sp_Columns_Delete";
        // Storage procedures for Business Logic
        public const string ReorderColumnsInBoard =
            "sp_Columns_ReorderInBoard";
    }

    public static class ForIssues
    {
        public const string Create =
            @"EXEC sp_Issues_Create
            @Title, @Description, @DueDate, @PositionInColumn,
            @ColumnId, @CreatorId, @AssigneeId, @CreatedAt";
        public const string Update =
            @"EXEC sp_Issues_Update 
            @Id, @Title, @Description, @DueDate, @AssigneeId";
        public const string GetById =
            "sp_Issues_GetById";
        public const string GetAll =
            "sp_Issues_GetAll";
        public const string GetByBoardIdInJson =
            "sp_Issues_GetByBoardIdJson";
        public const string GetByColumnId =
            "sp_Issues_GetByColumnId";
        public const string GetByColumnIdWithUsersInJson =
            "sp_Issues_GetByColumnIdWithUsersJson";
        public const string Exists =
            "sp_Issues_Exists";
        public const string Delete =
            "sp_Issues_Delete";
        // Storage procedures for Business Logic
        public const string ReorderIssuesInColumn =
            "sp_Issues_ReorderInColumn";
        public const string MoveIssue =
            "sp_Issues_Move";
    }
}