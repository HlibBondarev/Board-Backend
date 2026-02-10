using Board.BusinessLogic.DTOs.Users;
using Board.BusinessLogic.Features.ForUser.Commands;
using Board.BusinessLogic.Features.ForUser.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Board.WepAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IMediator mediator, ILogger<UserController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<UserController> _logger = logger;

    [HttpGet]
    public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
    {
        var users = await _mediator.Send(new GetAllUserQuery());

        return users ?? [];
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserResponseDto>> GetUser(int userId)
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

    [HttpPut("{userId}")]
    public async Task<ActionResult<UserResponseDto>> Update(int userId, UpdateUserCommand command)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(userId));
        if (user == null)
        {
            return NotFound();
        }
        var savedUser = await _mediator.Send(command);

        return savedUser;
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> Delete(int userId)
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
