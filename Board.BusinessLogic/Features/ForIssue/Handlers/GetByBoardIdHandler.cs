using Board.BusinessLogic.DTOs.Boards;
using Board.BusinessLogic.Features.ForIssue.Queries;
using Board.Common.Exceptions;
using Board.Common.Extensions;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Board.BusinessLogic.Features.ForIssue.Handlers;

public class GetByBoardIdHandler(
    IBoardRepository repository,
    ILogger<GetByBoardIdHandler> logger) : IRequestHandler<GetIssuesByBoardIdQuery, BoardHierarchyDto>
{
    public async Task<BoardHierarchyDto> Handle(GetIssuesByBoardIdQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing GetByBoardIdQuery for {Board} with {Id} in {GetByIdHandler}.",
            typeof(DataAccess.Models.Board).Name, request.Id, typeof(GetByBoardIdHandler));

        // 1. Fetch raw JSON string from the repository
        string? rawJson = await repository.GetBoardHierarchyInJson(request.Id, request.UserId);

        _ = rawJson ?? throw new NotFoundException($"{typeof(DataAccess.Models.Board).Name} with Id = {request.Id} not found");

        logger.LogInformation("Successfully completed executing GetBoardHierarchyRaw for {Board} with {Id} in {BoardRepository}.",
            typeof(DataAccess.Models.Board).Name, request.Id, typeof(IBoardRepository));

        try
        {
            // 2. Deserialize directly into the Business Layer DTO
            // This maintains clean architecture: Repository returns raw data, Service shapes it
            var boardDto = JsonSerializer.Deserialize<BoardHierarchyDto>(
                rawJson, new JsonSerializerOptions().GetDefault());

            // 3. Ensure collections are not null for the UI convenience
            _ = boardDto ?? throw new InvalidOperationException(
                $"Deserialization resulted in null for {typeof(BoardHierarchyDto).Name} with Id = {request.Id}.");

            boardDto.Columns ??= [];

            foreach (var col in boardDto.Columns)
            {
                col.Issues ??= [];
            }

            return boardDto;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to process board data structure.", ex);
        }
    }
}