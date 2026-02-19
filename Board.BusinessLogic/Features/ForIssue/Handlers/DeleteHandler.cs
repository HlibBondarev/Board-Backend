using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForIssue.Commands;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForIssue.Handlers;

public class DeleteHandler(
    IIssueRepository issueRepository,
    IColumnRepository columnRepository,
    ILogger<DeleteHandler> logger) : IRequestHandler<DeleteIssueCommand, IssuesByColumnIdResponseDto>
{
    public async Task<IssuesByColumnIdResponseDto> Handle(DeleteIssueCommand request, CancellationToken ct)
    {
        logger.LogInformation("Start deleting {Issue} with {Id} in {DeleteHandler}.",
            typeof(Issue).Name, request.Id, typeof(DeleteHandler).Name);

        // Fetch the issue to be deleted to ensure it exists.
        var issueToDelete = await issueRepository.GetById(request.Id) ??
            throw new NotFoundException(
            $"Failed to retrieve data for {typeof(Issue).Name} with {request.Id} in {typeof(DeleteHandler).Name}.");

        // Store the ColumnId before deletion for reordering and response purposes
        long columnId = issueToDelete.ColumnId;

        // Attempt to delete the issue
        bool isDeleted = await issueRepository.Delete(request.Id);

        if (isDeleted)
        {
            logger.LogInformation("Deleted {Issue} with {Id} in {DeleteHandler}.",
                typeof(Issue).Name, request.Id, typeof(DeleteHandler).Name);
        }
        else
        {
            logger.LogWarning("Failed to delete {Issue} with {Id} in {DeleteHandler}.",
                typeof(Issue).Name, request.Id, typeof(DeleteHandler).Name);
        }

        // Reorder remaining issues in the column after deletion
        await issueRepository.ReorderIssuesInColumnAsync(
            issueToDelete.PositionInColumn,
            columnId);

        // Fetch updated data for the column to return the current state after deletion
        (IEnumerable<Column> columns, IEnumerable<Issue>? issues, IEnumerable<User> users)
            = await columnRepository.GetAllWithIssuesAndUsers();

        // Ensure the requested column exists in the retrieved data
        var requestedColumnArr = new[] { columns.First(i => i.Id == columnId) };

        // Map to DTOs and extract issues for the specific column
        var issuesInColumn = MapToColumnResponseDto(requestedColumnArr, issues, users) ?? [];

        // Return the response with the updated list of issues for the column
        return new IssuesByColumnIdResponseDto(columnId, issuesInColumn.First(i => i.Id == columnId).Issues);
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
                    .Select(i => new IssueWithUserByColumnsResponseDto(
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