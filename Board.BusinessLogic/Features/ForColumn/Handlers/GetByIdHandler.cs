using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.Features.ForColumn.Queries;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForColumn.Handlers;

public class GetByIdHandler(
    IColumnRepository repository,
    ILogger<GetByIdHandler> logger) : IRequestHandler<GetColumnByIdQuery, ColumnResponseDto>
{
    public async Task<ColumnResponseDto> Handle(GetColumnByIdQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing GetByIdQuery for {Column} with {Id} in {GetByIdHandler}.",
            typeof(Column).Name, request.Id, typeof(GetByIdHandler));
        var column = await repository.GetById(request.Id);
        logger.LogInformation("Successfully completed executing GetByIdQuery for {Column} with {Id} in {ColumnRepository}.",
            typeof(Column).Name, column.Id, typeof(IColumnRepository));
        _ = column ?? throw new NotFoundException($"{typeof(Column).Name} with Id = {request.Id} not found");

        return column.ToDto();
    }
}
