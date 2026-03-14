using Board.BusinessLogic.DTOs.Boards;
using Board.BusinessLogic.Features.ForBoards.Commands;
using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForBoards.Handlers;

public class UpdateHandler(
    IBoardRepository repository,
    ILogger<UpdateHandler> logger) : IRequestHandler<UpdateBoardCommand, BoardResponseDto>
{
    public async Task<BoardResponseDto> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start updating {Board} with {Id} in {UpdateHandler}.",
            typeof(DataAccess.Models.Board).Name, request.BoardId, typeof(UpdateHandler));

        var board = await repository.GetById(request.BoardId);
        _ = board ?? throw new InvalidOperationException($"{typeof(DataAccess.Models.Board).Name} with Id = {request.BoardId} not found");

        board.SetToModel(request);

        _ = await repository.Update(board) ?? throw new InvalidOperationException(
            $"Updating {typeof(DataAccess.Models.Board).Name} with id={request.BoardId} failed.");

        logger.LogInformation("Successfully completed updating {Board} with {Id} in {ColumnRepository}.",
            typeof(DataAccess.Models.Board).Name, request.BoardId, typeof(IColumnRepository).Name);

        var result = await repository.GetById(request.BoardId) ?? throw new InvalidOperationException($"{typeof(DataAccess.Models.Board).Name} with Id = {request.BoardId} not found");

        return result.ToResponseDto();
    }
}