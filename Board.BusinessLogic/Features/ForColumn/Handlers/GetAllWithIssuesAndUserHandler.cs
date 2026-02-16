using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForColumn.Queries;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForColumn.Handlers;

public class GetAllWithIssuesAndUserHandler(
    IColumnRepository columnRepository,
    ILogger<GetAllWithIssuesAndUserHandler> logger) : IRequestHandler<GetAllColumnsWithIssuesAndUserQuery, IEnumerable<ColumnWithIssuesAndUserResponseDto>>
{
    public async Task<IEnumerable<ColumnWithIssuesAndUserResponseDto>> Handle(GetAllColumnsWithIssuesAndUserQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start executing GetAllWithIssuesAndUsersQuery for {Column}s, {Issue}s and {User}s in {GetAllWithIssuesAndUserHandler}.",
            typeof(Column).Name, typeof(Issue).Name, typeof(User).Name, typeof(GetAllWithIssuesAndUserHandler).Name);

        var (columns, issues, users) = await columnRepository.GetAllWithIssuesAndUsers();

        logger.LogInformation("Successfully completed executing GetAllWithIssuesAndUsers for {Column}s, {Issue}s and {User}s in {ColumnRepository}.",
            typeof(Column).Name, typeof(Issue).Name, typeof(User).Name, typeof(IColumnRepository).Name);

        return MapToColumnResponseDto(columns, issues, users);
    }

    private static IEnumerable<ColumnWithIssuesAndUserResponseDto> MapToColumnResponseDto(
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
            .Select(c => new ColumnWithIssuesAndUserResponseDto(
                c.Id,
                c.Name,
                c.Description,
                c.Position,
                c.UserId,
                GetDisplayName(c.UserId),
                [.. issuesLookup[c.Id]
                    .OrderBy(i => i.PositionInColumn)
                    .Select(i => new IssueByColumnWithUserResponseDto(
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

