using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForIssue.Queries;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForIssue.Handlers;

public class GetAllHandler(
    IIssueRepository repository,
    ILogger<GetAllHandler> logger) : IRequestHandler<GetAllIssuesQuery, IEnumerable<IssueResponseDto>>
{
    public async Task<IEnumerable<IssueResponseDto>> Handle(GetAllIssuesQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing GetAllQuery for {Issue}s in {GetAllHandler}.",
            typeof(Issue).Name, typeof(GetAllHandler));
        var issues = await repository.GetAll();
        logger.LogInformation("Successfully completed executing GetAllQuery for {Issue}s in {IssueRepository}.",
            typeof(Issue).Name, typeof(IIssueRepository));
        _ = issues ?? throw new NotFoundException($"No Issues found");

        return issues.ToDto();
    }
}
