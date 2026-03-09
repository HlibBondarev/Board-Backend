using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForIssue.Queries;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForIssue.Handlers;

public class GetByIdHandler(
    IIssueRepository repository,
    ILogger<GetByIdHandler> logger) : IRequestHandler<GetIssueByIdQuery, IssueResponseDto?>
{
    public async Task<IssueResponseDto?> Handle(GetIssueByIdQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing GetByIdQuery for {Issue} with {Id} in {GetByIdHandler}.",
            typeof(Issue).Name, request.Id, typeof(GetByIdHandler));

        var issue = await repository.GetById(request.Id);

        if (issue == null) { return null; }

        logger.LogInformation("Successfully completed executing GetByIdQuery for {Issue} with {Id} in {IssueRepository}.",
            typeof(Issue).Name, issue.Id, typeof(IIssueRepository));
        _ = issue ?? throw new NotFoundException($"{typeof(Issue).Name} with Id = {request.Id} not found");

        return issue.ToDto();
    }
}
