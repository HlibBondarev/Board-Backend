using Board.BusinessLogic.Features.ForBoards.Queries;
using Board.BusinessLogic.Features.ForIssue.Queries;
using Board.DataAccess.Models;
using Board.WepAPI.Authorization.Base;
using MediatR;

namespace Board.WepAPI.Authorization;

public class MustBeIssueCreatorOrAssigneeOrAdminHandler(
    IHttpContextAccessor httpContextAccessor,
    IMediator mediator) : MustBeBoardBaseHandler<MustBeIssueCreatorOrAssigneeOrAdminRequirement>(httpContextAccessor, mediator)
{
    protected override async Task<(bool, string?)> CheckMethod(long boardId, string userId, long? issueId = null)
    {
        if (issueId is null)
        {
            return (false, "Bad request: Could not determine Issue ID from request.");
        }

        var issueResponseDto = await mediator.Send(new GetIssueByIdQuery((long)issueId));
        if (issueResponseDto is null)
        {
            return (false, $"Bad request: {typeof(Issue).Name} with id={issueId} is not found.");
        }

        if (await mediator.Send(new CheckUserIsBoardAdminQuery(boardId, userId)))
        {
            return (true, null);
        }

        if (issueResponseDto.AssigneeId == userId
            || string.IsNullOrEmpty(issueResponseDto.AssigneeId)
            || issueResponseDto.CreatorId == userId)
        {
            return (true, null);
        }

        return (false, "Access denied: The User must be either the creator of the issue, the assignee, or the administrator of this board.");
    }
}