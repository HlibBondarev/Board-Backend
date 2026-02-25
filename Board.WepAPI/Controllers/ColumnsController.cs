using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.BusinessLogic.Features.ForColumn.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.WepAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ColumnsController(
    IMediator mediator,
    ILogger<ColumnsController> logger) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("{columnId}")]
    public async Task<ActionResult<ColumnResponseDto>> GetColumn(int columnId)
    {
        logger.LogInformation("Start  GetColumn action in {ColumnsController}.",
            typeof(ColumnsController));
        var column = await mediator.Send(new GetColumnByIdQuery(columnId));
        if (column == null)
        {
            return NotFound();
        }

        return column;
    }

    [HttpPost]
    public async Task<ActionResult<ColumnResponseDto>> Create(CreateColumnCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(result);
    }

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
