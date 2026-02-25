using Board.BusinessLogic.DTOs.Boards;
using MediatR;

namespace Board.BusinessLogic.Features.ForIssue.Queries;

public record GetIssuesByBoardIdQuery(long Id) : IRequest<BoardHierarchyDto>;