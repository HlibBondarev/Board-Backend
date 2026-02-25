using Board.BusinessLogic.DTOs.Boards;
using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.Features.ForBoards.Commands;
using Board.BusinessLogic.Features.ForBoards.Queries;
using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.BusinessLogic.Features.ForColumn.Queries;
using Board.BusinessLogic.Features.ForIssue.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.WepAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BoardsController(
    IMediator mediator,
    ILogger<BoardsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<BoardResponseDto>> GetBordsByUserId([FromQuery] string userId)
    {
        logger.LogInformation("Start  GetBordsByUserId action in {BoardsController}.",
            typeof(BoardsController));
        var boards = await mediator.Send(new GetBoardsByUserIdQuery(userId));

        return boards ?? [];
    }

    [HttpGet("{id}")]
    public async Task<BoardHierarchyDto> GetBordById(int id)
    {
        logger.LogInformation("Start  GetBordById action in {BoardsController}.",
            typeof(BoardsController));

        var board = await mediator.Send(new GetIssuesByBoardIdQuery(id));

        return board;
    }

    [HttpPost]
    public async Task<ActionResult<BoardCreateResponseDto>> Create(CreateBoardCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(result);
    }

    //[AllowAnonymous]
    //[HttpGet("{columnId}")]
    //public async Task<ActionResult<ColumnResponseDto>> GetColumn(int columnId)
    //{
    //    var column = await mediator.Send(new GetColumnByIdQuery(columnId));
    //    if (column == null)
    //    {
    //        return NotFound();
    //    }

    //    return column;
    //}

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