using Board.BusinessLogic.DTOs.Users;
using Board.BusinessLogic.Features.ForUser.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForUser.Handlers;

public class CreateHandler(
    IUserRepository repository,
    ILogger<CreateHandler> logger) : IRequestHandler<CreateUserCommand, UserResponseDto>
{
    public async Task<UserResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start creating {User} in {CreateHandler}.",
            typeof(User).Name, typeof(CreateHandler));
        User user = request.ToModel();
        User result = await repository.Create(user);
        logger.LogInformation("Successfully completed creating {User} with {Id} in {UserRepository}.",
            typeof(User).Name, result.Id, typeof(IUserRepository));

        return result.ToDto();
    }
}