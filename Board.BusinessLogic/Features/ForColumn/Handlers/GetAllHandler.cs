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
    private readonly IEntityRepositoryBase<int, Column> _repository = repository;
    private readonly ILogger<GetAllHandler> _logger = logger;

    public async Task<IEnumerable<ColumnResponseDto>> Handle(GetAllColumnsQuery request, CancellationToken ct)
    {
        _logger.LogInformation("Start executing GetAllQuery for {Column}s in UpdateHandler.", typeof(Column).Name);
        var columns = await _repository.GetAll(SqlStatements.ForColumns.GetAll);
        _ = columns ?? throw new NotFoundException($"No columns found");
        _logger.LogInformation("Successfully completed executing GetAllQuery for {Column}s in EntityRepository.", typeof(Column).Name);

        return columns.ToDto();
    }
}
