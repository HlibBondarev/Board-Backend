using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForColumn.Handlers;

public class DeleteHandler(IEntityRepositoryBase<int, Column> repository,
    ILogger<DeleteHandler> logger) : IRequestHandler<DeleteColumnCommand, bool>
{
    private readonly IEntityRepositoryBase<int, Column> _repository = repository;
    private readonly ILogger<DeleteHandler> _logger = logger;

    public async Task<bool> Handle(DeleteColumnCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Start deleting Column with {Id} in DeleteHandler.", request.Id);

        return await _repository.Delete(request.Id, SqlStatements.ForColumns.Delete);
    }
}