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
    ILogger<ColumnsController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<IssueResponseDto>> Create(CreateIssueCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult<IssueResponseDto>> Update(UpdateIssueCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<IssuesByColumnIdResponseDto>> Delete(int id)
    {
        var result = await mediator.Send(new DeleteIssueCommand(id));

        return Ok(result);
    }

    [HttpPatch]
    [Route("{id}/move")]
    public async Task<ActionResult> MoveIssue(long id, [FromBody] MoveIssueRequestDto dto)
    {
        logger.LogInformation("Start  moving {Issue} with {id} in MoveIssue action in {ColumnsController}.",
            typeof(Issue), id, typeof(ColumnsController));

        bool result = await mediator.Send(new MoveIssueCommand(id, dto.ColumnId, dto.Position));

        if (!result)
        {
            return BadRequest();
        }

        return NoContent();
    }
}
