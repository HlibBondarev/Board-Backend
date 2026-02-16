using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForIssue.Commands;
using Board.DataAccess.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Board.WepAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IssuesController(
    IMediator mediator,
    ILogger<ColumnsController> logger) : ControllerBase
{
    [HttpPatch]
    [Route("{id}/move")]
    public async Task<ActionResult<ColumnResponseDto>> MoveIssue(int id, [FromBody] MoveIssueRequestDto dto)
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
