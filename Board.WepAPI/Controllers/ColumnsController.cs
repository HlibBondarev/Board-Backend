using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.BusinessLogic.Features.ForColumn.Queries;
using Board.Common.Extensions;
using Board.Common.Services.Api;
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
    ICurrentUserService currentUserService,
    ILogger<ColumnsController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ColumnResponseDto>> Create(CreateColumnCommand command)
    {
        logger.LogInformation("Creating {column} in {ColumnsController}",
            typeof(Column).Name, typeof(ColumnsController).Name);

        var result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ColumnUpdateResponseDto>> Update(int id, UpdateColumnCommand command)
    {
        logger.LogInformation("Updating {column} with {Id} in {ColumnsController}",
            typeof(Column).Name, id, typeof(ColumnsController).Name);

        var finalCommand = command with { Id = id };
        var result = await mediator.Send(finalCommand);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<IssuesByColumnIdResponseDto>> Delete(int id)
    {
        logger.LogInformation("Deleting {column} with id={id} in Delete action of {BoardsController}",
           typeof(Column).Name, id, typeof(BoardsController).Name);

        string userId = await this.GetUserId(currentUserService);
        var result = await mediator.Send(new DeleteColumnCommand(id, userId));

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ColumnResponseDto>> GetColumnById(int id)
    {
        logger.LogInformation("Start getting {column} with id={id} in GetColumnById action of {ColumnsController}.",
            typeof(Column).Name, id, typeof(ColumnsController).Name);

        var column = await mediator.Send(new GetColumnByIdQuery(id));
        if (column == null)
        {
            return NotFound();
        }

        return column;
    }
}
