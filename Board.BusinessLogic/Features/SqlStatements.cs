namespace Board.BusinessLogic.Features;

public static class SqlStatements
{
    public static class ForUsers
    {
        public const string Create =
            @"EXEC dbo.User_Post @Email = @Email, @DisplayName = @DisplayName";
        public const string GetById =
            @"EXEC dbo.User_GetSingle @Id = @Id";
        public const string GetAll =
            @"dbo.User_GetAll";
        public const string Any =
            @"dbo.User_Any";
        public const string Update =
            @"EXEC dbo.User_Put @Id = @Id, @Email = @Email, @DisplayName = @DisplayName";
        public const string Delete =
           @"EXEC dbo.User_Delete @Id = @Id";
    }

    public static class ForColumns
    {
        public const string Create =
            @"XXXXX";
        public const string GetById =
            @"EXEC dbo.Column_GetSingle @Id = @Id";
        public const string GetAll =
             @"dbo.Column_GetAll";
        public const string Any =
            @"XXXXX";
        public const string Update =
            @"XXXXX";
        public const string Delete =
            @"XXXXX";
    }

    public static class ForIssues
    {
        public const string Create =
            @"XXXXX";
        public const string GetById =
            @"EXEC dbo.Issue_GetSingle @Id = @Id";
        public const string GetAll =
             @"dbo.Issue_GetAll";
        public const string Any =
            @"XXXXX";
        public const string Update =
            @"XXXXX";
        public const string Delete =
            @"XXXXX";
    }
}