using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForIssue.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForIssue.Handlers;

public class UpdateHandler(
    IIssueRepository repository,
    ILogger<UpdateHandler> logger) : IRequestHandler<UpdateIssueCommand, IssueResponseDto>
{
    public async Task<IssueResponseDto> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start updating {Issue} with {Id} in {UpdateHandler}.",
            typeof(Issue).Name, request.Id, typeof(UpdateHandler));
        Issue issue = request.ToModel();
        Issue result = await repository.Update(issue);
        logger.LogInformation("Successfully completed updating {Issue} with {Id} in {IssueRepository}.",
            typeof(Issue).Name, request.Id, typeof(IIssueRepository));

        return result.ToDto();
    }
}