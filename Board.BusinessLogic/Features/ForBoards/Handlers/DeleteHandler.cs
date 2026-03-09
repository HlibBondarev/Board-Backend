using Board.BusinessLogic.Features.ForBoards.Commands;
using Board.Common.Exceptions;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForBoards.Handlers;

public class DeleteHandler(
    IBoardRepository boardRepository,
    ILogger<DeleteHandler> logger) : IRequestHandler<DeleteBoardCommand>
{
    public async Task Handle(DeleteBoardCommand request, CancellationToken ct)
    {
        logger.LogInformation("Start deleting {Board} with {Id} in {DeleteHandler}.",
            typeof(DataAccess.Models.Board).Name, request.Id, typeof(DeleteHandler).Name);

        // Fetch the issue to be deleted to ensure it exists.
        _ = await boardRepository.GetById(request.Id) ??
            throw new NotFoundException(
            $"Failed to retrieve data for {typeof(DataAccess.Models.Board).Name} with {request.Id} in {typeof(DeleteHandler).Name}.");

        // Attempt to delete the column
        bool isDeleted = await boardRepository.Delete(request.Id);

        if (!isDeleted)
        {
            logger.LogError("Failed to delete {Board} with {Id} in {DeleteHandler}.",
                typeof(DataAccess.Models.Board).Name, request.Id, typeof(DeleteHandler).Name);
            throw new InvalidOperationException(
                $"Failed to delete {typeof(DataAccess.Models.Board).Name} with {request.Id} in {typeof(DeleteHandler).Name}.");
        }

        logger.LogInformation("Deleted {Board} with {Id} in {DeleteHandler}.",
                typeof(DataAccess.Models.Board).Name, request.Id, typeof(DeleteHandler).Name);
    }
}