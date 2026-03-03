using Board.BusinessLogic.DTOs.Users;
using Board.BusinessLogic.Features.ForUser.Commands;
using Board.BusinessLogic.Features.ForUser.Queries;
using Board.DataAccess.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.WepAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController(
    IMediator mediator,
    ILogger<UsersController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
    {
        logger.LogInformation("Start  GetAllUsers action in {UsersController}.",
            typeof(UsersController));

        var users = await mediator.Send(new GetAllUsersQuery());

        return users ?? [];
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDto>> GetUserById(string id)
    {
        logger.LogInformation("Start getting {User} with id={userId} in GetAllUsers action of {UsersController}.",
            typeof(User), id, typeof(UsersController));

        var user = await mediator.Send(new GetUserByIdQuery(id));
        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> Create(CreateUserCommand command)
    {
        logger.LogInformation("Start creating a {user} in Create action in {UsersController}",
            typeof(User).Name, typeof(UsersController).Name);

        var result = await mediator.Send(command);

        return Ok(result);
    }

    [Authorize(Policy = "MustBeThisUser")]
    [HttpPut("{userId}")]
    public async Task<ActionResult<UserResponseDto>> Update(string userId, UpdateUserCommand command)
    {
        logger.LogInformation("Start updating the {user} with id={userId} in Update action in {UsersController}",
            typeof(User).Name, userId, typeof(UsersController).Name);

        var user = await mediator.Send(new GetUserByIdQuery(userId));
        if (user == null)
        {
            return NotFound();
        }

        var result = await mediator.Send(command);

        return Ok(result);
    }

    [Authorize(Policy = "MustBeThisUser")]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> Delete(string userId)
    {
        logger.LogInformation("Start deleting the {user} with id={userId} in Delete action in {UsersController}",
            typeof(User).Name, userId, typeof(UsersController).Name);

        var user = await mediator.Send(new GetUserByIdQuery(userId));
        if (user == null)
        {
            return NotFound();
        }
        await mediator.Send(new DeleteUserCommand(userId));

        return NoContent();
    }
}
