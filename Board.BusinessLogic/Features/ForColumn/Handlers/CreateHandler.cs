using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForColumn.Handlers;

public class CreateHandler(IEntityRepositoryBase<int, Column> repository,
    ILogger<CreateHandler> logger) : IRequestHandler<CreateColumnCommand, ColumnResponseDto>
{
    private readonly IEntityRepositoryBase<int, Column> _repository = repository;
    private readonly ILogger<CreateHandler> _logger = logger;

    public async Task<ColumnResponseDto> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start creating {Column} in UpdateHandler.", typeof(Column).Name);
        Column column = request.ToModel();
        Column result = await _repository.Create(column, SqlStatements.ForColumns.Create);
        _logger.LogInformation("Successfully completed creating {Column} with {Id} in EntityRepository.",
            typeof(Column).Name, result.Id);

        return result.ToDto();
    }
}