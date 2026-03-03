using Board.BusinessLogic.DTOs.Boards;
using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.Features.ForBoards.Commands;
using Board.BusinessLogic.Features.ForBoards.Queries;
using Board.BusinessLogic.Features.ForColumn.Commands;
using Board.BusinessLogic.Features.ForIssue.Queries;
using Board.BusinessLogic.Features.ForUser.Commands;
using Board.BusinessLogic.Features.ForUser.Queries;
using Board.Common;
using Board.Common.Extensions;
using Board.Common.Services.Api;
using Board.DataAccess.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace Board.WepAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BoardsController(
    IMediator mediator,
    ICurrentUserService currentUserService,
    ILogger<BoardsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<BoardResponseDto>> GetBordsByUserId()
    {
        logger.LogInformation("Start  GetBordsByUserId action in {BoardsController}.",
            typeof(BoardsController));

        var userClaims = await this.GetUserClaims(currentUserService);
        var userId = userClaims.Id ?? throw new AuthenticationException(
                    $"Can not get user's claim {nameof(IdentityResourceClaimsTypes.Sub)} from Context.");
        var user = await mediator.Send(new GetUserByIdQuery(userId));

        if (user == null)
        {
            await mediator.Send(new CreateUserCommand(userId, userClaims.Email, userClaims.Name));
            return [];
        }

        var boards = await mediator.Send(new GetBoardsByUserIdQuery(userId));

        return boards ?? [];
    }

    [HttpGet("{id}")]
    public async Task<BoardHierarchyDto> GetBordById(long id)
    {
        logger.LogInformation("Start  GetBordById action for {Board} with id={id} in {BoardsController}.",
            typeof(DataAccess.Models.Board).Name, id, typeof(BoardsController).Name);

        string userId = await this.GetUserId(currentUserService);
        var board = await mediator.Send(new GetIssuesByBoardIdQuery(id, userId));

        return board;
    }

    [HttpPost]
    public async Task<ActionResult<BoardCreateResponseDto>> Create(CreateBoardCommand command)
    {
        logger.LogInformation("Adding a new board in Create action of {BoardsController}", typeof(BoardsController).Name);

        string userId = await this.GetUserId(currentUserService);
        var finalCommand = command with { UserId = userId };
        var result = await mediator.Send(finalCommand);

        return Ok(result);
    }

    [HttpPost("{boardId}/columns")]
    public async Task<ActionResult<ColumnResponseDto>> AddColumnToBoard(int boardId, [FromBody] CreateColumnCommand command)
    {
        logger.LogInformation("Adding a new {column} to board with id={BoardId}", typeof(Column), boardId);

        var finalCommand = command with { BoardId = boardId };
        var result = await mediator.Send(finalCommand);

        return Ok(result);
    }

    [HttpPost("{boardId}/members")]
    public async Task<ActionResult> AddUserToBoard(long boardId, [FromBody] AddUserToBoardCommand command)
    {
        logger.LogInformation(
            "Adding the User with email = {Email} to Board with id = {BoardId}", command.Email, boardId);

        var finalCommand = command with { BoardId = boardId };
        await mediator.Send(finalCommand);

        return Ok(new { message = "The User has been successfully added to the Board." });
    }

    [HttpDelete("{boardId}/members")]
    public async Task<ActionResult> RemoveUserFromBoard(int boardId, [FromBody] RemoveUserFromBoardCommand command)
    {
        logger.LogInformation(
            "Removing the User with email = {Email} from Board with id = {BoardId}",
            command.Email, boardId);

        var finalCommand = command with { BoardId = boardId };
        await mediator.Send(finalCommand);

        return Ok(new { message = "The User has been successfully removed from the Board." });
    }
}