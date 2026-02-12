using Board.BusinessLogic.DTOs.Users;
using Board.BusinessLogic.Features.ForUser.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForUser.Handlers;

public class CreateHandler(IEntityRepositoryBase<string, User> repository,
    ILogger<CreateHandler> logger) : IRequestHandler<CreateUserCommand, UserResponseDto>
{
    public async Task<UserResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start creating {User} in UpdateHandler.", typeof(User).Name);
        User user = request.ToModel();
        User result = await repository.Create(user, SqlStatements.ForUsers.Create);
        logger.LogInformation("Successfully completed creating {User} with {Id} in EntityRepository.",
            typeof(User).Name, result.Id);

        return result.ToDto();
    }
}