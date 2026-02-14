using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForColumn.Queries;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForColumn.Handlers;

public class GetAllWithIssuesHandler(
    IEntityRepositoryBase<int, Column> columnRepository,
    ILogger<GetAllWithIssuesHandler> logger) : IRequestHandler<GetAllColumnsWithIssuesQuery, IEnumerable<ColumnResponseWithIssuesDto>>
{
    public async Task<IEnumerable<ColumnResponseWithIssuesDto>> Handle(GetAllColumnsWithIssuesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start executing GetAllQuery for {Column}s, {Issue}s and {User}s in GetAllWithIssuesHandler.",
            typeof(Column).Name, typeof(Issue).Name, typeof(User).Name);
        IEnumerable<Column> columns = [];
        IEnumerable<Issue> issues = [];
        IEnumerable<User> users = [];

        // 1. Fetch all raw data sets in ONE round-trip to the database
        await columnRepository.QueryMultipleAsync(SqlStatements.ForColumns.GetIssuesByColumnsForUsers, null, async multi =>
        {
            // ReadAsync ensures the web server threads are not blocked during I/O
            columns = await multi.ReadAsync<Column>();
            issues = await multi.ReadAsync<Issue>();
            users = await multi.ReadAsync<User>();
        });

        // 2. Perform high-performance mapping in memory
        return MapToColumnResponseDto(columns, issues, users);


        //var columns = await columnRepository.GetAll(SqlStatements.ForColumns.GetAll);
        //logger.LogInformation("Successfully completed executing GetAllQuery for {Column}s in EntityRepository.", typeof(Column).Name);
        //_ = columns ?? throw new NotFoundException($"No columns found");

        //logger.LogInformation("Start executing GetAllQuery for {Issue}s in UpdateHandler.", typeof(Issue).Name);
        //IEnumerable<Issue> issues = await issueRepository.GetAll(SqlStatements.ForIssues.GetAll) ?? [];
        //logger.LogInformation("Successfully completed executing GetAllQuery for {Issue}s in EntityRepository.", typeof(Issue).Name);

        //return MapToColumnResponseDto(columns, issues, users);
    }


    private static IEnumerable<ColumnResponseWithIssuesDto> MapToColumnResponseDto(
    IEnumerable<Column> columns,
    IEnumerable<Issue> issues,
    IEnumerable<User> users)
    {
        // Guard clauses
        columns ??= [];
        issues ??= [];
        users ??= [];

        // 1. Create a fast O(N) lookup for issues (grouped by ColumnId)
        var issuesLookup = issues.ToLookup(i => i.ColumnId);

        // 2. Create a Dictionary for O(1) user display name lookups
        var usersDict = users.ToDictionary(u => u.Id, u => u.DisplayName);

        // Helper to resolve DisplayName safely
        string GetDisplayName(string id) =>
            usersDict.TryGetValue(id, out var name) ? name : "Unknown User";

        // 3. Project entities into the final DTO structure
        return [.. columns
            .OrderBy(c => c.Position)
            .Select(c => new ColumnResponseWithIssuesDto(
                c.Id,
                c.Name,
                c.Description,
                c.Position,
                c.UserId,
                GetDisplayName(c.UserId),
                [.. issuesLookup[c.Id]
                    .OrderBy(i => i.PositionInColumn)
                    .Select(i => new IssueWithUserNameResponseDto(
                        i.Id,
                        i.Title,
                        i.Description,
                        i.DueDate,
                        i.CreatedAt,
                        i.PositionInColumn,
                        i.ColumnId,
                        i.CreatorId,
                        GetDisplayName(i.CreatorId),
                        i.AssigneeId,
                        i.AssigneeId != null ? GetDisplayName(i.AssigneeId) : null
                    ))]
            ))];
    }
}



//private static IEnumerable<ColumnResponseWithIssuesDto> MapToColumnResponseDto(
//IEnumerable<Column> columns,
//IEnumerable<Issue> issues)
//{
//    // 1. Guard clauses to prevent null reference exceptions
//    columns ??= [];
//    issues ??= [];

//    // 2. Create a fast O(N) lookup to group issues by ColumnId
//    // This avoids nested loops and significantly improves performance
//    var issuesLookup = issues.ToLookup(i => i.ColumnId);

//    // 3. Transform entities into DTOs using LINQ
//    return [.. columns
//        .OrderBy(c => c.Position) // Ensure columns follow the specified order
//        .Select(c => new ColumnResponseWithIssuesDto(
//            c.Id,
//            c.Name,
//            c.Description,
//            c.Position,
//            c.UserId,
//            [.. issuesLookup[c.Id] // Efficiently retrieve issues for the current column
//                .OrderBy(i => i.PositionInColumn) // Sort issues within the column
//                .Select(i => new IssueResponseDto(
//                    i.Id,
//                    i.Title,
//                    i.Description,
//                    i.DueDate,
//                    i.CreatedAt,
//                    i.PositionInColumn,
//                    i.ColumnId,
//                    i.CreatorId,
//                    i.AssigneeId
//                ))] // Materialize the sub-collection into a list
//        ))]; // Materialize the final collection
//}
//}
