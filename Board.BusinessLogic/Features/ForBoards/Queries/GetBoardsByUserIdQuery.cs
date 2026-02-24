using Board.BusinessLogic.DTOs.Boards;
using MediatR;

namespace Board.BusinessLogic.Features.ForBoards.Queries;

public record GetBoardsByUserIdQuery(string Id) : IRequest<IEnumerable<BoardResponseDto>>;