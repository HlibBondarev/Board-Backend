using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForIssue.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForIssue.Handlers;

public class UpdateHandler(IEntityRepositoryBase<int, Issue> repository,
    ILogger<UpdateHandler> logger) : IRequestHandler<UpdateIssueCommand, IssueResponseDto>
{
    private readonly IEntityRepositoryBase<int, Issue> _repository = repository;
    private readonly ILogger<UpdateHandler> _logger = logger;

    public async Task<IssueResponseDto> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start updating {Issue} with {Id} in UpdateHandler.",
            typeof(Issue).Name, request.Id);
        Issue issue = request.ToModel();
        Issue result = await _repository.Update(issue, SqlStatements.ForIssues.Update);
        _logger.LogInformation("Successfully completed updating {Issue} with {Id} in EntityRepository.",
            typeof(Issue).Name, request.Id);

        return result.ToDto();
    }
}