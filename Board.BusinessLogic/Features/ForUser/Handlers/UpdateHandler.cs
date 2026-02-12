using Board.BusinessLogic.DTOs.Users;
using Board.BusinessLogic.Features.ForUser.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForUser.Handlers;

public class UpdateHandler(IEntityRepositoryBase<string, User> repository,
    ILogger<UpdateHandler> logger) : IRequestHandler<UpdateUserCommand, UserResponseDto>
{
    public async Task<UserResponseDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start updating {User} with {Id} in UpdateHandler.", typeof(User).Name, request.Id);
        User user = request.ToModel();
        User result = await repository.Update(user, SqlStatements.ForUsers.Update);
        logger.LogInformation("Successfully completed updating {User} with {Id} in EntityRepository.", typeof(User).Name, request.Id);

        return result.ToDto();
    }
}