using Board.BusinessLogic.Features.ForColumn.Queries;
using Board.BusinessLogic.Features.ForIssue.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Board.WepAPI.Authorization.Base;

public abstract class MustBeBoardBaseHandler<T>(
    IHttpContextAccessor httpContextAccessor,
    IMediator mediator) : AuthorizationHandler<T> where T : IAuthorizationRequirement
{
    protected readonly IMediator mediator = mediator;

    // Template method
    protected override sealed async Task HandleRequirementAsync(AuthorizationHandlerContext context, T requirement)
    {
        // 1. Check authentication (Early Return)
        if (context.User.Identity?.IsAuthenticated != true)
        {
            var reason = "Unauthorized user: Authentication required.";
            context.Fail(new AuthorizationFailureReason(this, reason));
        }

        // 2. Get data
        var routeValues = httpContextAccessor.HttpContext?.Request.RouteValues;
        if (routeValues == null) return;

        long? boardId = await GetBoardIdFromRoute(routeValues);

        if (!boardId.HasValue)
        {
            context.Fail(new AuthorizationFailureReason(this, "Bad request: Could not determine Board ID from request."));
            return;
        }

        string userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        long? issueId = null;
        if (routeValues.TryGetValue("issueId", out var iId) && long.TryParse(iId?.ToString(), out var longIssueId))
        {
            issueId = longIssueId;
        }

        // 3. Business logic only in the overridden method
        (bool condition, string? message) = await CheckMethod((long)boardId, userId, issueId);

        if (condition)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail(new AuthorizationFailureReason(this, message!));
        }
    }

    protected abstract Task<(bool condition, string? message)> CheckMethod(long boardId, string userId, long? issueId = null);

    protected virtual async Task<long?> GetBoardIdFromRoute(RouteValueDictionary routeValues)
    {
        if (routeValues.TryGetValue("boardId", out var bId) && long.TryParse(bId?.ToString(), out var boardId))
        {
            return boardId;
        }

        if (routeValues.TryGetValue("columnId", out var cId) && long.TryParse(cId?.ToString(), out var columnId))
        {
            return (await mediator.Send(new GetColumnByIdQuery(columnId))).BoardId;
        }

        if (routeValues.TryGetValue("issueId", out var iId) && long.TryParse(iId?.ToString(), out var issueId))
        {
            long columnWithIssueId = (await mediator.Send(new GetIssueByIdQuery(issueId))).ColumnId;
            return (await mediator.Send(new GetColumnByIdQuery(columnWithIssueId))).BoardId;
        }

        return null;
    }
}