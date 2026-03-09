using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForColumn.Handlers;

public class UpdateHandler(
    IColumnRepository repository,
    ILogger<UpdateHandler> logger) : IRequestHandler<UpdateColumnCommand, bool>
{
    public async Task<bool> Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start updating {Column} with {Id} in {UpdateHandler}.",
            typeof(Column).Name, request.ColumnId, typeof(UpdateHandler));

        var column = await repository.GetById(request.ColumnId);
        _ = column ?? throw new BadRequestException($"{typeof(Column).Name} with Id = {request.ColumnId} not found");

        column.SetToModel(request);

        _ = await repository.Update(column) ?? throw new InvalidOperationException(
            $"Updating {typeof(Column).Name} with id={request.ColumnId} failed.");

        logger.LogInformation("Successfully completed updating {Column} with {Id} in {ColumnRepository}.",
            typeof(Column).Name, request.ColumnId, typeof(IColumnRepository).Name);

        return true;
    }
}