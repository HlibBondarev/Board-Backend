using MediatR;

namespace Board.BusinessLogic.Features.ForIssue.Commands;

public record DeleteIssueCommand(int Id) : IRequest<bool>;