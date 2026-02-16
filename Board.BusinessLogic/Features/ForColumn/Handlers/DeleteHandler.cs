using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForColumn.Handlers;

public class DeleteHandler(
    IColumnRepository repository,
    ILogger<DeleteHandler> logger) : IRequestHandler<DeleteColumnCommand, bool>
{
    public async Task<bool> Handle(DeleteColumnCommand request, CancellationToken ct)
    {
        logger.LogInformation("Start deleting {Column} with {Id} in {DeleteHandler}.",
            typeof(Column).Name, request.Id, typeof(DeleteHandler).Name);

        return await repository.Delete(request.Id);
    }
}