using Board.BusinessLogic.DTOs.Issues;
using MediatR;

namespace Board.BusinessLogic.Features.ForIssue.Queries;

public record GetIssueByIdQuery(int Id) : IRequest<IssueResponseDto>;