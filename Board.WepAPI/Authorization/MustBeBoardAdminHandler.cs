using Board.BusinessLogic.Features.ForBoards.Queries;
using Board.WepAPI.Authorization.Base;
using MediatR;

namespace Board.WepAPI.Authorization;

public class MustBeBoardAdminHandler(
    IHttpContextAccessor httpContextAccessor,
    IMediator mediator) : MustBeBoardBaseHandler<MustBeBoardAdminRequirement>(httpContextAccessor, mediator)
{
    protected override async Task<(bool, string?)> CheckMethod(long boardId, string userId, long? issueId = null)
    {
        bool condition = await mediator.Send(new CheckUserIsBoardAdminQuery(boardId, userId));
        if (condition)
        {
            return (true, null);
        }
        return (false, "Access denied: The user is not an administrator of this board.");
    }
}