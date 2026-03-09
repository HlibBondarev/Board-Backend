using Board.BusinessLogic.Features.ForBoards.Queries;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForBoards.Handlers;

public class CheckBoardMembershipHandler(
    IBoardRepository boardRepository,
    ILogger<CheckBoardMembershipHandler> logger) : IRequestHandler<CheckBoardMembershipQuery, bool>
{
    public async Task<bool> Handle(CheckBoardMembershipQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing CheckBoardMembershipQuery for {Board} with {boardId} and {User} with {userId} in {CheckBoardMembershipHandler}.",
            typeof(DataAccess.Models.Board).Name, request.BoardId, typeof(User).Name, request.UserId, typeof(CheckBoardMembershipHandler).Name);

        return await boardRepository.CheckBoardMembershipByUserId(request.BoardId, request.UserId);
    }
}