using Board.BusinessLogic.DTOs.Boards;
using Board.BusinessLogic.Features.ForBoards.Queries;
using Board.Common.Extensions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Board.BusinessLogic.Features.ForBoards.Handlers;

public class GetByUserIdHandler(
    IBoardRepository boardRepository,
    ILogger<GetByUserIdHandler> logger) : IRequestHandler<GetBoardsByUserIdQuery, IEnumerable<BoardResponseDto>>
{
    public async Task<IEnumerable<BoardResponseDto>> Handle(GetBoardsByUserIdQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing GetByUserIdQuery for {Board} with {Id} in {GetByUserIdHandler}.",
            typeof(DataAccess.Models.Board).Name, request.Id, typeof(GetByUserIdHandler).Name);
        string? rawJson = await boardRepository.GetByUserIdInJson(request.Id);
        logger.LogInformation("Successfully completed executing GetByIdUserQuery for {User} with {Id} in {BoardRepository}.",
            typeof(User).Name, request.Id, typeof(IBoardRepository).Name);

        if (rawJson is null)
        {
            logger.LogInformation("{Board}s for {typeof(User).Name} with Id = {request.Id} not found",
            typeof(DataAccess.Models.Board).Name, typeof(User).Name, request.Id);
            return [];
        }

        try
        {
            var boards = JsonSerializer.Deserialize<IEnumerable<BoardResponseDto>>(rawJson, new JsonSerializerOptions().GetDefault());
            _ = boards ?? throw new InvalidOperationException(
                $"Deserialization resulted in null for {typeof(BoardResponseDto).Name} with UserId = {request.Id}.");

            return boards;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to process boards data structure.", ex);
        }
    }
}