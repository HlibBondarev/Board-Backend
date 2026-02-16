using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.BusinessLogic.Features.ForColumn.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.WepAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ColumnsController(
    IMediator mediator,
    ILogger<ColumnsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<ColumnWithIssuesAndUserResponseDto>> GetAllColumns()
    {
        logger.LogInformation("Start  GetAllColumns action in {ColumnsController}.",
            typeof(ColumnsController));
        var columns = await mediator.Send(new GetAllColumnsWithIssuesAndUserQuery());

        return columns ?? [];
    }

    [HttpGet("{columnId}")]
    public async Task<ActionResult<ColumnResponseDto>> GetColumn(int columnId)
    {
        var column = await mediator.Send(new GetColumnByIdQuery(columnId));
        if (column == null)
        {
            return NotFound();
        }

        return column;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ColumnResponseDto>> Create(CreateColumnCommand command)
    {
        var createdColumn = await mediator.Send(command);

        return CreatedAtAction(nameof(GetColumn), new
        {
            userId = createdColumn.Id
        }, createdColumn);
    }

    [Authorize]
    [HttpPut("{columnId}")]
    public async Task<ActionResult<ColumnResponseDto>> Update(int columnId, UpdateColumnCommand command)
    {
        var column = await mediator.Send(new GetColumnByIdQuery(columnId));
        if (column == null)
        {
            return NotFound();
        }
        var savedColumn = await mediator.Send(command);

        return savedColumn;
    }

    [Authorize]
    [HttpDelete("{columnId}")]
    public async Task<IActionResult> Delete(int columnId)
    {
        var column = await mediator.Send(new GetColumnByIdQuery(columnId));
        if (column == null)
        {
            return NotFound();
        }
        await mediator.Send(new DeleteColumnCommand(columnId));

        return NoContent();
    }
}
