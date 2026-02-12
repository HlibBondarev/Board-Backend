using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForColumn.Handlers;

public class UpdateHandler(IEntityRepositoryBase<int, Column> repository,
    ILogger<UpdateHandler> logger) : IRequestHandler<UpdateColumnCommand, ColumnResponseDto>
{
    public async Task<ColumnResponseDto> Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start updating {Column} with {Id} in UpdateHandler.",
            typeof(Column).Name, request.Id);
        Column column = request.ToModel();
        Column result = await repository.Update(column, SqlStatements.ForColumns.Update);
        logger.LogInformation("Successfully completed updating {Column} with {Id} in EntityRepository.",
            typeof(Column).Name, request.Id);

        return result.ToDto();
    }
}