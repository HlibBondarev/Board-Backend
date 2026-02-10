using MediatR;

namespace Board.BusinessLogic.Features.ForUser.Commands;

public record DeleteUserCommand(int Id) : IRequest<bool>;