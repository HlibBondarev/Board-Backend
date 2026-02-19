using MediatR;

namespace Board.BusinessLogic.Features.ForUser.Commands;

public record DeleteUserCommand(string Id) : IRequest<bool>;