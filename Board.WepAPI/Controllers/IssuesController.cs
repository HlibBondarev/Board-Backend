using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForIssue.Commands;
using Board.DataAccess.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.WepAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class IssuesController(
    IMediator mediator,
    ILogger<IssuesController> logger) : ControllerBase
{
    [HttpPut]
    [Route("{issueId}")]
    [Authorize(Policy = "MustBeIssueAssigneeOrAdminOrIssueAssigneeIsNull")]
    public async Task<ActionResult<bool>> Update(long issueId, UpdateIssueCommand command)
    {
        logger.LogInformation("Start  updating {Issue} with {id} in Update action in {IssuesController}.",
            typeof(Issue).Name, command.IssueId, typeof(IssuesController).Name);

        var finalCommand = command with { IssueId = issueId };

        var isUpdated = await mediator.Send(finalCommand);

        return Ok(isUpdated);
    }

    [HttpDelete]
    [Route("{issueId}")]
    [Authorize(Policy = "MustBeIssueCreatorOrAssigneeOrAdmin")]
    public async Task<ActionResult<IssuesByColumnIdResponseDto>> Delete(long issueId)
    {
        logger.LogInformation("Start  deleting {Issue} with {id} in Delete action in {IssuesController}.",
            typeof(Issue).Name, issueId, typeof(IssuesController).Name);

        var result = await mediator.Send(new DeleteIssueCommand(issueId));

        return Ok(result);
    }

    [HttpPatch]
    [Route("{issueId}/move")]
    [Authorize(Policy = "MustBeMemberOfBoard")]
    public async Task<ActionResult> MoveIssue(long issueId, [FromBody] MoveIssueRequestDto dto)
    {
        logger.LogInformation("Start  moving {Issue} with {id} in MoveIssue action in {IssuesController}.",
            typeof(Issue).Name, issueId, typeof(IssuesController).Name);

        bool isMoved = await mediator.Send(new MoveIssueCommand(issueId, dto.ColumnId, dto.Position));

        if (!isMoved)
        {
            return BadRequest();
        }

        return NoContent();
    }
}
