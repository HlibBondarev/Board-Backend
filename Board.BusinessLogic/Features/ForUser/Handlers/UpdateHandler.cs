using Board.BusinessLogic.DTOs.Users;
using Board.BusinessLogic.Features.ForUser.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForUser.Handlers;

public class UpdateHandler(IEntityRepositoryBase<int, User> repository,
    ILogger<UpdateHandler> logger) : IRequestHandler<UpdateUserCommand, UserResponseDto>
{
    private readonly IEntityRepositoryBase<int, User> _repository = repository;
    private readonly ILogger<UpdateHandler> _logger = logger;

    public async Task<UserResponseDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start updating {User} with {Id} in UpdateHandler.", typeof(User).Name, request.Id);
        User user = request.ToModel();
        User result = await _repository.Update(user, SqlStatements.ForUsers.Update);
        _logger.LogInformation("Successfully completed updating {User} with {Id} in EntityRepository.", typeof(User).Name, request.Id);

        return result.ToDto();
    }
}