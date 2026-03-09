using Board.BusinessLogic.Features.ForBoards.Queries;
using Board.WepAPI.Authorization.Base;
using MediatR;

namespace Board.WepAPI.Authorization;

public class MustBeMemberOfBoardHandler(
    IHttpContextAccessor httpContextAccessor,
    IMediator mediator) : MustBeBoardBaseHandler<MustBeMemberOfBoardRequirement>(httpContextAccessor, mediator)
{
    protected override async Task<(bool, string?)> CheckMethod(long boardId, string userId, long? issueId = null)
    {
        bool condition = await mediator.Send(new CheckBoardMembershipQuery(boardId, userId));
        if (condition)
        {
            return (true, null);
        }
        return (false, "Access denied: The user is not a member of this board.");
    }
}
