using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForColumn.Handlers;

public class UpdateHandler(
    IColumnRepository repository,
    ILogger<UpdateHandler> logger) : IRequestHandler<UpdateColumnCommand, ColumnUpdateResponseDto>
{
    public async Task<ColumnUpdateResponseDto> Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start updating {Column} with {Id} in {UpdateHandler}.",
            typeof(Column).Name, request.Id, typeof(UpdateHandler));

        var column = await repository.GetById(request.Id);
        _ = column ?? throw new BadRequestException($"{typeof(Column).Name} with Id = {request.Id} not found");
        column.SetToModel(request);
        var updatedColumn = await repository.Update(column);

        logger.LogInformation("Successfully completed updating {Column} with {Id} in {ColumnRepository}.",
            typeof(Column).Name, request.Id, typeof(IColumnRepository));

        return updatedColumn.ToUpdateDto();
    }
}