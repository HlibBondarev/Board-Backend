using MediatR;

namespace Board.BusinessLogic.Features.ForColumn.Commands;

public record DeleteColumnCommand(int Id) : IRequest<bool>;