using Board.BusinessLogic.DTOs.Users;
using Board.BusinessLogic.Features.ForUser.Commands;
using Board.BusinessLogic.Features.ForUser.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.WepAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController(IMediator mediator, ILogger<UsersController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<UsersController> _logger = logger;

    [AllowAnonymous]
    [HttpGet]
    public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
    {
        var users = await _mediator.Send(new GetAllUsersQuery());

        return users ?? [];
    }

    [AllowAnonymous]
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserResponseDto>> GetUser(string userId)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(userId));
        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> Create(CreateUserCommand command)
    {
        var createdUser = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetUser), new
        {
            userId = createdUser.Id
        }, createdUser);
    }

    [Authorize(Policy = "MustBeThisUser")]
    [HttpPut("{userId}")]
    public async Task<ActionResult<UserResponseDto>> Update(string userId, UpdateUserCommand command)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(userId));
        if (user == null)
        {
            return NotFound();
        }
        var savedUser = await _mediator.Send(command);

        return savedUser;
    }

    [Authorize(Policy = "MustBeThisUser")]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> Delete(string userId)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(userId));
        if (user == null)
        {
            return NotFound();
        }
        await _mediator.Send(new DeleteUserCommand(userId));

        return NoContent();
    }
}
