using Board.BusinessLogic.DTOs.Columns;
using MediatR;

namespace Board.BusinessLogic.Features.ForColumn.Queries;

public record GetAllColumnsWithIssuesQuery : IRequest<IEnumerable<ColumnResponseWithIssuesDto>>;