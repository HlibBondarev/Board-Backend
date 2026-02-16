using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForColumn.Handlers;

public class UpdateHandler(
    IColumnRepository repository,
    ILogger<UpdateHandler> logger) : IRequestHandler<UpdateColumnCommand, ColumnResponseDto>
{
    public async Task<ColumnResponseDto> Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start updating {Column} with {Id} in {UpdateHandler}.",
            typeof(Column).Name, request.Id, typeof(UpdateHandler));
        Column column = request.ToModel();
        Column result = await repository.Update(column);
        logger.LogInformation("Successfully completed updating {Column} with {Id} in {ColumnRepository}.",
            typeof(Column).Name, request.Id, typeof(IColumnRepository));

        return result.ToDto();
    }
}