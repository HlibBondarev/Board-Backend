using Board.BusinessLogic.Features.ForIssue.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForIssue.Handlers;

public class MoveIssueHandler(
    IIssueRepository repository,
    ILogger<MoveIssueHandler> logger) : IRequestHandler<MoveIssueCommand, bool>
{
    public async Task<bool> Handle(MoveIssueCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start move {Issue} with {IssueId} in {MoveIssueHandler}.",
            typeof(Issue).Name, request.IssueId, typeof(MoveIssueHandler));

        return await repository.MoveIssueAsync(request.IssueId, request.TargetColumnId, request.NewPosition);
    }
}