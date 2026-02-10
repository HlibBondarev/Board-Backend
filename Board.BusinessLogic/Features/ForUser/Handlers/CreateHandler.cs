using Board.BusinessLogic.DTOs.Users;
using Board.BusinessLogic.Features.ForUser.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForUser.Handlers;

public class CreateHandler(IEntityRepositoryBase<int, User> repository,
    ILogger<CreateHandler> logger) : IRequestHandler<CreateUserCommand, UserResponseDto>
{
    private readonly IEntityRepositoryBase<int, User> _repository = repository;
    private readonly ILogger<CreateHandler> _logger = logger;

    public async Task<UserResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start creating User in UpdateHandler.");
        var user = new User { Email = request.Email, DisplayName = request.DisplayName };
        var result = await _repository.Create(user, SqlStatements.ForUsers.Create);
        _logger.LogInformation("Successfully completed creating User with {Id} in EntityRepository.", result.Id);

        return result.ToDto();
    }
}