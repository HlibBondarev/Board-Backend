using Board.BusinessLogic.DTOs.Boards;
using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Board.BusinessLogic.Features.ForColumn.Handlers;

public class DeleteHandler(
    IColumnRepository columnRepository,
    IBoardRepository boardRepository,
    ILogger<DeleteHandler> logger) : IRequestHandler<DeleteColumnCommand, IEnumerable<ColumnHierarchyDto>>
{
    public async Task<IEnumerable<ColumnHierarchyDto>> Handle(DeleteColumnCommand request, CancellationToken ct)
    {
        logger.LogInformation("Start deleting {Column} with {Id} in {DeleteHandler}.",
            typeof(Column).Name, request.Id, typeof(DeleteHandler).Name);

        // Fetch the issue to be deleted to ensure it exists.
        var columnToDelete = await columnRepository.GetById(request.Id) ??
            throw new NotFoundException(
            $"Failed to retrieve data for {typeof(Column).Name} with {request.Id} in {typeof(DeleteHandler).Name}.");

        // Attempt to delete the column
        bool isDeleted = await columnRepository.Delete(request.Id);

        if (isDeleted)
        {
            logger.LogInformation("Deleted {Column} with {Id} in {DeleteHandler}.",
                typeof(Column).Name, request.Id, typeof(DeleteHandler).Name);
        }
        else
        {
            logger.LogError("Failed to delete {Column} with {Id} in {DeleteHandler}.",
                typeof(Column).Name, request.Id, typeof(DeleteHandler).Name);
            throw new BadRequestException
                ($"Failed to delete {typeof(Column).Name} with {request.Id} in {typeof(DeleteHandler).Name}.");
        }

        // Reorder remaining columns in the board after deletion
        bool isReordered = await columnRepository.ReorderColumnsInBoard(
            columnToDelete.Position,
            columnToDelete.BoardId);

        if (!isReordered)
        {
            logger.LogError(
                "Failed to reorder {Column}s in {Board} after deleting {Column} with {Id} in {DeleteHandler}.",
                typeof(Column).Name, typeof(DataAccess.Models.Board).Name, typeof(Column).Name, request.Id, typeof(DeleteHandler).Name);
            throw new BadRequestException
                ($"Failed to reorder {typeof(Column).Name}s after deleting {typeof(Column).Name} with {request.Id} in {typeof(DeleteHandler).Name}.");
        }

        // 1. Fetch raw JSON string from the repository
        string? rawJson = await boardRepository.GetBoardHierarchyRaw(columnToDelete.BoardId, request.UserId);

        _ = rawJson ?? throw new NotFoundException($"{typeof(DataAccess.Models.Board).Name} with Id = {request.Id} not found");

        logger.LogInformation("Successfully completed executing GetBoardHierarchyRaw for {Board} with {Id} in {BoardRepository}.",
            typeof(DataAccess.Models.Board).Name, request.Id, typeof(IBoardRepository));

        try
        {
            // 2. Deserialize directly into the Business Layer DTO
            // This maintains clean architecture: Repository returns raw data, Service shapes it
            var boardDto = JsonSerializer.Deserialize<BoardHierarchyDto>(rawJson, jsonOptions);

            // 3. Ensure collections are not null for the UI convenience
            _ = boardDto ?? throw new InvalidOperationException(
                $"Deserialization resulted in null for {typeof(BoardHierarchyDto).Name} with Id = {request.Id}.");

            boardDto.Columns ??= [];

            foreach (var col in boardDto.Columns)
            {
                col.Issues ??= [];
            }

            return boardDto.Columns;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to process board data structure.", ex);
        }
    }

    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };
}