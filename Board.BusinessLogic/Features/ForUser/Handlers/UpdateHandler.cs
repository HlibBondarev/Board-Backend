using Board.BusinessLogic.DTOs.Users;
using Board.BusinessLogic.Features.ForUser.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForUser.Handlers;

public class UpdateHandler(IUserRepository repository,
    ILogger<UpdateHandler> logger) : IRequestHandler<UpdateUserCommand, UserResponseDto>
{
    public async Task<UserResponseDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start updating {User} with {Id} in {UpdateHandler}.",
            typeof(User).Name, request.Id, typeof(UpdateHandler));
        User user = request.ToModel();
        User result = await repository.Update(user);
        logger.LogInformation("Successfully completed updating {User} with {Id} in {UserRepository}.",
            typeof(User).Name, request.Id, typeof(IUserRepository));

        return result.ToDto();
    }
}