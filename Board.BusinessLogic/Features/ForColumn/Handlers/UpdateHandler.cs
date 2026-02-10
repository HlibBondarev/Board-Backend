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
    private readonly IEntityRepositoryBase<int, Column> _repository = repository;
    private readonly ILogger<UpdateHandler> _logger = logger;

    public async Task<ColumnResponseDto> Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start updating {Column} with {Id} in UpdateHandler.",
            typeof(Column).Name, request.Id);
        var column = new Column { Id = request.Id, Name = request.Name, Position = request.Position };
        var result = await _repository.Update(column, SqlStatements.ForColumns.Update);
        _logger.LogInformation("Successfully completed updating {Column} with {Id} in EntityRepository.",
            typeof(Column).Name, request.Id);

        return result.ToDto();
    }
}