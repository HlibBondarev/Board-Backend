using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.BusinessLogic.Features.ForColumn.Queries;
using Board.DataAccess.Models;
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
    [HttpPost]
    public async Task<ActionResult<ColumnResponseDto>> Create(CreateColumnCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ColumnUpdateResponseDto>> Update(int id, UpdateColumnCommand command)
    {
        logger.LogInformation("Updating {column} with {Id}", typeof(Column).Name, id);

        // Use the 'with' keyword to create a new instance of the record 
        // with the id provided from the route segment
        var finalCommand = command with { Id = id };

        var result = await mediator.Send(finalCommand);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<IssuesByColumnIdResponseDto>> Delete(int id, [FromQuery] string userId)
    {
        var result = await mediator.Send(new DeleteColumnCommand(id, userId));

        return Ok(result);
    }

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
}
