using MediatR;

namespace Board.BusinessLogic.Features.ForUser.Commands;

public record Delete(int Id) : IRequest<bool>;