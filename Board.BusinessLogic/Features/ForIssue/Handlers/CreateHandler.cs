using Board.BusinessLogic.Features.ForIssue.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForIssue.Handlers;

public class CreateHandler(
    IIssueRepository repository,
    ILogger<CreateHandler> logger) : IRequestHandler<CreateIssueCommand, long>
{
    public async Task<long> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start creating {Issue} in {CreateHandler}.",
            typeof(Issue).Name, typeof(CreateHandler).Name);

        Issue issue = request.ToModel();
        Issue result = await repository.Create(issue);

        logger.LogInformation("Successfully completed creating {Issue} with {Id} in {IssueRepository}.",
            typeof(Issue).Name, result.Id, typeof(IIssueRepository).Name);

        return result.Id;
    }
}