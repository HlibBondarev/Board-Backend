using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForIssue.Commands;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        bool isReordered = await issueRepository.ReorderIssuesInColumnAsync(
            issueToDelete.PositionInColumn,
            columnId);

        if (!isReordered)
        {
            logger.LogWarning(
                "Failed to reorder {Issue}s in {Column} after deleting {Issue} with {Id} in {DeleteHandler}.",
                typeof(Issue).Name, typeof(Column).Name, typeof(Issue).Name, request.Id, typeof(DeleteHandler).Name);
        }

        // 1. Fetch raw JSON string from the repository
        string? rawJson = await columnRepository.GetIssuesInColumnRaw(columnId);

        _ = rawJson ?? throw new NotFoundException($"{typeof(Column)} with Id = {columnId} not found");

        logger.LogInformation(
            "Successfully completed executing GetIssuesInColumnRaw  query for {Column} with {Id} in {ColumnRepository}.",
            typeof(Column).Name, request.Id, typeof(IColumnRepository));

        try
        {
            // 2. Deserialize directly into the Business Layer DTO
            // This maintains clean architecture: Repository returns raw data, Service shapes it
            var issuesInColumn = JsonSerializer.Deserialize<IEnumerable<IssueWithUserByColumnsResponseDto>>(rawJson, jsonOptions);

            // 3. Ensure collections are not null for the UI convenience
            _ = issuesInColumn ?? throw new InvalidOperationException(
                $"Deserialization resulted in null for {typeof(IEnumerable<IssueWithUserByColumnsResponseDto>).Name} with Id = {request.Id}.");

            return new IssuesByColumnIdResponseDto(columnId, issuesInColumn);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"Failed to process {typeof(IEnumerable<IssueWithUserByColumnsResponseDto>).Name} data structure.", ex);
        }
    }

    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };
}