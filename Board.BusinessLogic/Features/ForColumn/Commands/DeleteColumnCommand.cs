using MediatR;

namespace Board.BusinessLogic.Features.ForColumn.Commands;

public record DeleteColumnCommand(long Id) : IRequest<bool>;