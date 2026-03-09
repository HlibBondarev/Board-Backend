using Board.BusinessLogic.Features.ForIssue.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForIssue.Handlers;

public class UpdateHandler(
    IIssueRepository repository,
    ILogger<UpdateHandler> logger) : IRequestHandler<UpdateIssueCommand, bool>
{
    public async Task<bool> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start updating {Issue} with {Id} in {UpdateHandler}.",
            typeof(Issue).Name, request.IssueId, typeof(UpdateHandler).Name);

        var issue = await repository.GetById(request.IssueId) ?? throw new InvalidOperationException(
                $"{typeof(Issue).Name} with id={request.IssueId} is not found. Updating this {typeof(Issue).Name} failed.");

        request.SetToModel(issue);

        _ = await repository.Update(issue) ?? throw new InvalidOperationException(
            $"Updating {typeof(Issue).Name} with id={request.IssueId} failed.");

        logger.LogInformation("Successfully completed updating {Issue} with {Id} in {IssueRepository}.",
            typeof(Issue).Name, request.IssueId, typeof(IIssueRepository).Name);

        return true;
    }
}