using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForIssue.Queries;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForIssue.Handlers;

public class GetByIdHandler(IEntityRepositoryBase<int, Issue> repository,
    ILogger<GetByIdHandler> logger) : IRequestHandler<GetIssueByIdQuery, IssueResponseDto>
{
    public async Task<IssueResponseDto> Handle(GetIssueByIdQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing GetByIdQuery for {Issue} with {Id} in GetByIdHandler.",
            typeof(Issue).Name, request.Id);
        var issue = await repository.GetById(request.Id, SqlStatements.ForIssues.GetById);
        logger.LogInformation("Successfully completed executing GetByIdQuery for {Issue} with {Id} in EntityRepository.",
            typeof(Issue).Name, issue.Id);
        _ = issue ?? throw new NotFoundException($"{typeof(Column).Name} with Id = {request.Id} not found");

        return issue.ToDto();
    }
}
