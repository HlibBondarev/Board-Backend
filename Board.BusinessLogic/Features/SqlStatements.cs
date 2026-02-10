namespace Board.BusinessLogic.Features;

public static class SqlStatements
{
    public static class ForUsers
    {
        public const string Create =
            @"EXEC dbo.User_Post @Email = @Email, @DisplayName = @DisplayName, @CreatedAt = @CreatedAt";
        public const string GetById =
            @"EXEC dbo.User_GetSingle @Id = @Id";
        public const string GetAll =
            @"dbo.User_GetAll";
        public const string Any =
            @"dbo.User_Any";
        public const string Update =
            @"EXEC dbo.User_Put @Id = @Id, @Email = @Email, @DisplayName = @DisplayName, @CreatedAt = @CreatedAt";
        public const string Delete =
           @"EXEC dbo.User_Delete @Id = @Id";
    }

    public static class ForColumns
    {
        public const string Create =
            @"EXEC dbo.Column_Post @Name = @Name, @Description = @Description, @Position = @Position, @UserId = @UserId";
        public const string GetById =
            @"EXEC dbo.Column_GetSingle @Id = @Id";
        public const string GetAll =
             @"dbo.Column_GetAll";
        public const string Any =
            @"dbo.Column_Any";
        public const string Update =
            @"EXEC dbo.Column_Put @Id = @Id, @Name = @Name, @Description = @Description, @Position = @Position, @UserId = @UserId";
        public const string Delete =
           @"EXEC dbo.Column_Delete @Id = @Id";
    }

    public static class ForIssues
    {
        public const string Create = @"EXEC dbo.Issue_Post 
            @Title = @Title, @Description = @Description,
            @DueDate = @DueDate, @CreatedAt = @CreatedAt,
            @PositionInColumn = @PositionInColumn, @ColumnId = @ColumnId,
            @CreatorId = @CreatorId, @AssigneeId = @AssigneeId";
        public const string GetById =
            @"EXEC dbo.Issue_GetSingle @Id = @Id";
        public const string GetAll =
             @"dbo.Issue_GetAll";
        public const string Any =
            @"dbo.Issue_Any";
        public const string Update = @"EXEC dbo.Issue_Put 
            @Title = @Title, @Description = @Description,
            @DueDate = @DueDate, @CreatedAt = @CreatedAt,
            @PositionInColumn = @PositionInColumn, @ColumnId = @ColumnId,
            @CreatorId = @CreatorId, @AssigneeId = @AssigneeId";
        public const string Delete =
            @"EXEC dbo.Issue_Delete @Id = @Id";
    }
}