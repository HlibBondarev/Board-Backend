using Board.BusinessLogic.Features.ForIssue.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForIssue.Handlers;

public class DeleteHandler(
    IIssueRepository repository,
    ILogger<DeleteHandler> logger) : IRequestHandler<DeleteIssueCommand, bool>
{
    public async Task<bool> Handle(DeleteIssueCommand request, CancellationToken ct)
    {
        logger.LogInformation("Start deleting {Issue} with {Id} in {DeleteHandler}.",
            typeof(Issue).Name, request.Id, typeof(DeleteHandler));

        return await repository.Delete(request.Id);
    }
}