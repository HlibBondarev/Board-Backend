using Board.BusinessLogic.DTOs.Boards;
using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.Features.ForBoards.Commands;
using Board.BusinessLogic.Features.ForBoards.Queries;
using Board.BusinessLogic.Features.ForColumn.Commands;
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
    public async Task<BoardHierarchyDto> GetBordById(long id, [FromQuery] string userId)
    {
        logger.LogInformation("Start  GetBordById action in {BoardsController}.",
            typeof(BoardsController));

        var board = await mediator.Send(new GetIssuesByBoardIdQuery(id, userId));

        return board;
    }

    [HttpPost]
    public async Task<ActionResult<BoardCreateResponseDto>> Create(CreateBoardCommand command)
    {
        var result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("{boardId}/columns")]
    public async Task<ActionResult<ColumnResponseDto>> AddColumn(int boardId, [FromBody] CreateColumnCommand command)
    {
        logger.LogInformation("Adding a new column to board {BoardId}", boardId);
        var finalCommand = command with { BoardId = boardId };

        var result = await mediator.Send(finalCommand);

        return Ok(result);
    }

    [HttpPost("{boardId}/members")]
    public async Task<ActionResult> AddUserToBoard(long boardId, [FromBody] AddUserToBoardCommand command)
    {
        logger.LogInformation("Adding the User to Board with id = {BoardId}", boardId);
        var finalCommand = command with { BoardId = boardId };

        await mediator.Send(finalCommand);

        return Ok(new { message = "User added successfully" });
    }
}