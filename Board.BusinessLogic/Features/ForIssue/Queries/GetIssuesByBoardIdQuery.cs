using Board.BusinessLogic.DTOs.Boards;
using MediatR;

namespace Board.BusinessLogic.Features.ForIssue.Queries;

public record GetIssuesByBoardIdQuery(int Id) : IRequest<BoardHierarchyDto>;