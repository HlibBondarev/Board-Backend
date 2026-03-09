using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.BusinessLogic.Features.ForIssue.Commands;
using Board.Common.Extensions;
using Board.Common.Services.Api;
using Board.DataAccess.Models;
using Board.WepAPI.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.WepAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ColumnsController(
    IMediator mediator,
    ICurrentUserService currentUserService,
    ILogger<ColumnsController> logger) : ControllerBase
{
    [HttpPut("{columnId}")]
    [ValidateId(nameof(columnId))]
    [Authorize(Policy = "MustBeMemberOfBoard")]
    public async Task<ActionResult<bool>> Update(int columnId, UpdateColumnCommand command)
    {
        logger.LogInformation("Updating {column} with {Id} in {ColumnsController}",
            typeof(Column).Name, columnId, typeof(ColumnsController).Name);

        var finalCommand = command with { ColumnId = columnId };

        var isUpdated = await mediator.Send(finalCommand);

        return Ok(isUpdated);
    }

    [HttpDelete("{columnId}")]
    [ValidateId(nameof(columnId))]
    [Authorize(Policy = "MustBeBoardAdmin")]
    public async Task<ActionResult<IssuesByColumnIdResponseDto>> Delete(int columnId)
    {
        logger.LogInformation("Deleting {column} with id={id} in Delete action of {ColumnsController}",
           typeof(Column).Name, columnId, typeof(ColumnsController).Name);

        string userId = await this.GetUserId(currentUserService);
        var result = await mediator.Send(new DeleteColumnCommand(columnId, userId));

        return Ok(result);
    }

    [HttpPost("{columnId}/issues")]
    [ValidateId(nameof(columnId))]
    [Authorize(Policy = "MustBeMemberOfBoard")]
    public async Task<ActionResult<long>> AddIssueToColumn(
        int columnId, [FromBody] CreateIssueCommand command)
    {
        logger.LogInformation(
            "Start  creating {Issue} in Create action in {ColumnsController}.",
            typeof(Issue), typeof(ColumnsController));

        var finalCommand = command with { ColumnId = columnId };
        var createdIssueId = await mediator.Send(finalCommand);

        return Ok(createdIssueId);
    }
}
