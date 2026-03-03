using Board.BusinessLogic.DTOs.Boards;
using Board.BusinessLogic.Features.ForBoards.Queries;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Board.BusinessLogic.Features.ForBoards.Handlers;

public class GetByUserIdHandler(
    IBoardRepository repository,
    ILogger<GetByUserIdHandler> logger) : IRequestHandler<GetBoardsByUserIdQuery, IEnumerable<BoardResponseDto>>
{
    public async Task<IEnumerable<BoardResponseDto>> Handle(GetBoardsByUserIdQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing GetByUserIdQuery for {Board} with {Id} in {GetByUserIdHandler}.",
            typeof(DataAccess.Models.Board).Name, request.Id, typeof(GetByUserIdHandler));
        string? rawJson = await repository.GetByUserIdRaw(request.Id);
        logger.LogInformation("Successfully completed executing GetByIdUserQuery for {User} with {Id} in {BoardRepository}.",
            typeof(DataAccess.Models.Board).Name, request.Id, typeof(IBoardRepository));

        if (rawJson is null)
        {
            logger.LogInformation("{typeof(DataAccess.Models.Board).Name}s for {typeof(User).Name} with Id = {request.Id} not found",
            typeof(DataAccess.Models.Board).Name, typeof(User).Name, request.Id);
            return [];
        }

        try
        {
            var boards = JsonSerializer.Deserialize<IEnumerable<BoardResponseDto>>(rawJson, jsonOptions);
            _ = boards ?? throw new InvalidOperationException(
                $"Deserialization resulted in null for {typeof(BoardResponseDto).Name} with UserId = {request.Id}.");

            return boards;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to process boards data structure.", ex);
        }
    }

    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };
}