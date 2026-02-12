using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.BusinessLogic.Features.ForColumn.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.WepAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ColumnsController(IMediator mediator, ILogger<ColumnsController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<ColumnsController> _logger = logger;

    [HttpGet]
    public async Task<IEnumerable<ColumnResponseWithIssuesDto>> GetAllColumns()
    {
        var columns = await _mediator.Send(new GetAllColumnsWithIssuesQuery());

        return columns ?? [];
    }

    [HttpGet("{columnId}")]
    public async Task<ActionResult<ColumnResponseDto>> GetColumn(int columnId)
    {
        var column = await _mediator.Send(new GetColumnByIdQuery(columnId));
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
        var createdColumn = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetColumn), new
        {
            userId = createdColumn.Id
        }, createdColumn);
    }

    [Authorize]
    [HttpPut("{columnId}")]
    public async Task<ActionResult<ColumnResponseDto>> Update(int columnId, UpdateColumnCommand command)
    {
        var column = await _mediator.Send(new GetColumnByIdQuery(columnId));
        if (column == null)
        {
            return NotFound();
        }
        var savedColumn = await _mediator.Send(command);

        return savedColumn;
    }

    [Authorize]
    [HttpDelete("{columnId}")]
    public async Task<IActionResult> Delete(int columnId)
    {
        var column = await _mediator.Send(new GetColumnByIdQuery(columnId));
        if (column == null)
        {
            return NotFound();
        }
        await _mediator.Send(new DeleteColumnCommand(columnId));

        return NoContent();
    }
}
