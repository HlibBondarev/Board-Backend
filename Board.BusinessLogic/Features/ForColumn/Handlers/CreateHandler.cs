using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForColumn.Handlers;

public class CreateHandler(
    IColumnRepository repository,
    ILogger<CreateHandler> logger) : IRequestHandler<CreateColumnCommand, ColumnCreateResponseDto>
{
    public async Task<ColumnCreateResponseDto> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start creating {Column} in {CreateHandler}.", typeof(Column).Name, typeof(CreateHandler).Name);
        Column column = request.ToModel();
        Column result = await repository.Create(column);
        logger.LogInformation("Successfully completed creating {Column} with {Id} in {ColumnRepository}.",
            typeof(Column).Name, result.Id, typeof(IColumnRepository).Name);

        return result.ToCreateDto();
    }
}