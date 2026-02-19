using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForIssue.Commands;

public record MoveIssueCommand(
    [Required]
    long IssueId,

    [Required]
    long TargetColumnId,

    [Required]
    int NewPosition
) : IRequest<bool>;