using Board.BusinessLogic.DTOs.Columns;
using MediatR;

namespace Board.BusinessLogic.Features.ForColumn.Commands;

public record DeleteColumnCommand(long Id, string UserId) : IRequest<IEnumerable<ColumnHierarchyDto>>;