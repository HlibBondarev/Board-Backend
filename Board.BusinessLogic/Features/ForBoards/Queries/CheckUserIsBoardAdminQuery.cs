using MediatR;

namespace Board.BusinessLogic.Features.ForBoards.Queries;

public record CheckUserIsBoardAdminQuery(long BoardId, string UserId) : IRequest<bool>;