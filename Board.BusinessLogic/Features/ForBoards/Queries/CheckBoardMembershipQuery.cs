using MediatR;

namespace Board.BusinessLogic.Features.ForBoards.Queries;

public record CheckBoardMembershipQuery(long BoardId, string UserId) : IRequest<bool>;