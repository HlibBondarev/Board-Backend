using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForIssue.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForIssue.Handlers;

public class CreateHandler(IEntityRepositoryBase<int, Issue> repository,
    ILogger<CreateHandler> logger) : IRequestHandler<CreateIssueCommand, IssueResponseDto>
{
    public async Task<IssueResponseDto> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start creating {Issue} in UpdateHandler.", typeof(Issue).Name);
        Issue issue = request.ToModel();
        Issue result = await repository.Create(issue, SqlStatements.ForIssues.Create);
        logger.LogInformation("Successfully completed creating {Issue} with {Id} in EntityRepository.",
            typeof(Issue).Name, result.Id);

        return result.ToDto();
    }
}