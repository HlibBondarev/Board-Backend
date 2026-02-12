using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.Features.ForColumn.Queries;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForColumn.Handlers;

public class GetByIdHandler(IEntityRepositoryBase<int, Column> repository,
    ILogger<GetByIdHandler> logger) : IRequestHandler<GetColumnByIdQuery, ColumnResponseDto>
{
    public async Task<ColumnResponseDto> Handle(GetColumnByIdQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing GetByIdQuery for {Column} with {Id} in GetByIdHandler.",
            typeof(Column).Name, request.Id);
        var column = await repository.GetById(request.Id, SqlStatements.ForColumns.GetById);
        _ = column ?? throw new NotFoundException($"{typeof(Column).Name} with Id = {request.Id} not found");
        logger.LogInformation("Successfully completed executing GetByIdQuery for {Column} with {Id} in EntityRepository.",
            typeof(Column).Name, column.Id);

        return column.ToDto();
    }
}
