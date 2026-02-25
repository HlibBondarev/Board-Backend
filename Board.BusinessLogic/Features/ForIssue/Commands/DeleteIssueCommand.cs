using Board.BusinessLogic.DTOs.Issues;
using MediatR;

namespace Board.BusinessLogic.Features.ForIssue.Commands;

public record DeleteIssueCommand(long Id) : IRequest<IssuesByColumnIdResponseDto>;