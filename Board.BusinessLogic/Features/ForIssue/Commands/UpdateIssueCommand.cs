using Board.BusinessLogic.DTOs.Issues;
using Board.DataAccess.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.BusinessLogic.Features.ForIssue.Commands;

public record UpdateIssueCommand(
    [Required]
    long Id,

    [Required]
    [StringLength(200, MinimumLength = 3)]
    string Title,

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    string Description,

    [DataType(DataType.Date)]
    DateTime? DueDate,

    string? AssigneeId
) : IRequest<IssueResponseDto>;

public static class UpdateColumnCommandExtensions
{
    public static void SetToModel(this UpdateIssueCommand dto, Issue model)
    {
        model.Title = dto.Title;
        model.Description = dto.Description;
        model.DueDate = dto.DueDate;
        model.AssigneeId = dto.AssigneeId;
    }
}
