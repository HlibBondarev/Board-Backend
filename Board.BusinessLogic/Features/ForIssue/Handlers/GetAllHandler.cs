using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForIssue.Queries;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForIssue.Handlers;

public class GetAllHandler(IEntityRepositoryBase<int, Issue> repository,
    ILogger<GetAllHandler> logger) : IRequestHandler<GetAllIssuesQuery, IEnumerable<IssueResponseDto>>
{
    public async Task<IEnumerable<IssueResponseDto>> Handle(GetAllIssuesQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing GetAllQuery for {Issue}s in UpdateHandler.", typeof(Issue).Name);
        var issues = await repository.GetAll(SqlStatements.ForIssues.GetAll);
        _ = issues ?? throw new NotFoundException($"No Issues found");
        logger.LogInformation("Successfully completed executing GetAllQuery for {Issue}s in EntityRepository.", typeof(Issue).Name);

        return issues.ToDto();
    }
}
