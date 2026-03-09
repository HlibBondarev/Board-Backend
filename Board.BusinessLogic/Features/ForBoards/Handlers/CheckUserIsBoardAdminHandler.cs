using Board.BusinessLogic.Features.ForBoards.Queries;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForBoards.Handlers;

public class CheckUserIsBoardAdminHandler(
    IBoardRepository boardRepository,
    ILogger<CheckUserIsBoardAdminHandler> logger) : IRequestHandler<CheckUserIsBoardAdminQuery, bool>
{
    public async Task<bool> Handle(CheckUserIsBoardAdminQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing CheckUserIsBoardAdminHandler for {Board} with {boardId} and {User} with {userId} in {CheckUserIsBoardAdminHandler}.",
            typeof(DataAccess.Models.Board).Name, request.BoardId, typeof(User).Name, request.UserId, typeof(CheckUserIsBoardAdminHandler).Name);

        return await boardRepository.CheckUserIsBoardAdmin(request.BoardId, request.UserId);
    }
}