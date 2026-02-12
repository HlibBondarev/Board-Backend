using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.Features.ForColumn.Queries;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForColumn.Handlers;

public class GetAllHandler(IEntityRepositoryBase<int, Column> repository,
    ILogger<GetAllHandler> logger) : IRequestHandler<GetAllColumnsQuery, IEnumerable<ColumnResponseDto>>
{
    public async Task<IEnumerable<ColumnResponseDto>> Handle(GetAllColumnsQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing GetAllQuery for {Column}s in GetAllHandler.", typeof(Column).Name);
        var columns = await repository.GetAll(SqlStatements.ForColumns.GetAll);
        logger.LogInformation("Successfully completed executing GetAllQuery for {Column}s in EntityRepository.", typeof(Column).Name);
        _ = columns ?? throw new NotFoundException($"No columns found");

        return columns.ToDto();
    }
}
