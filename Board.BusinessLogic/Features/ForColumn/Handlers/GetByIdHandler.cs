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
    private readonly IEntityRepositoryBase<int, Column> _repository = repository;
    private readonly ILogger<GetByIdHandler> _logger = logger;

    public async Task<ColumnResponseDto> Handle(GetColumnByIdQuery request, CancellationToken ct)
    {
        _logger.LogInformation("Start executing GetByIdQuery for Column with {Id} in GetByIdHandler.", request.Id);
        var column = await _repository.GetById(request.Id, SqlStatements.ForColumns.GetById);
        _ = column ?? throw new NotFoundException($"Column with Id = {request.Id} not found");
        _logger.LogInformation("Successfully completed executing GetByIdQuery for Column with {Id} in EntityRepository.", column.Id);

        return column.ToDto();
    }
}
