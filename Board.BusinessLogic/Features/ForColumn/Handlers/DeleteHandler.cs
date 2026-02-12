using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForColumn.Handlers;

public class DeleteHandler(IEntityRepositoryBase<int, Column> repository,
    ILogger<DeleteHandler> logger) : IRequestHandler<DeleteColumnCommand, bool>
{
    public async Task<bool> Handle(DeleteColumnCommand request, CancellationToken ct)
    {
        logger.LogInformation("Start deleting {Column} with {Id} in DeleteHandler.", typeof(Column).Name, request.Id);

        return await repository.Delete(request.Id, SqlStatements.ForColumns.Delete);
    }
}