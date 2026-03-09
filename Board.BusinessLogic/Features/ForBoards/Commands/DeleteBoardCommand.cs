using MediatR;

namespace Board.BusinessLogic.Features.ForBoards.Commands;

public record DeleteBoardCommand(long Id) : IRequest;