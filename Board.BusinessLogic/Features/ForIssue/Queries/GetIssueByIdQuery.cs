using Board.BusinessLogic.DTOs.Issues;
using MediatR;

namespace Board.BusinessLogic.Features.ForIssue.Queries;

public record GetIssueByIdQuery(long Id) : IRequest<IssueResponseDto>;