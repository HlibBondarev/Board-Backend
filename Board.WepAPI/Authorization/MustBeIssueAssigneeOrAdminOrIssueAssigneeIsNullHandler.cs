using Board.BusinessLogic.Features.ForBoards.Queries;
using Board.BusinessLogic.Features.ForIssue.Queries;
using Board.DataAccess.Models;
using Board.WepAPI.Authorization.Base;
using MediatR;

namespace Board.WepAPI.Authorization;

public class MustBeIssueAssigneeOrAdminOrIssueAssigneeIsNullHandler(
    IHttpContextAccessor httpContextAccessor,
    IMediator mediator) : MustBeBoardBaseHandler<MustBeIssueAssigneeOrAdminOrIssueAssigneeIsNullRequirement>(httpContextAccessor, mediator)
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

        if (string.IsNullOrEmpty(issueResponseDto.AssigneeId) || issueResponseDto.AssigneeId == userId)
        {
            return (true, null);
        }

        return (false, "Access denied: Either no one is assigned to this issue, or the User must be assigned to this issue, or the User is an administrator of this board.");
    }
}