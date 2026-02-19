using Board.BusinessLogic.Features.ForUser.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForUser.Handlers;

public class DeleteHandler(
    IUserRepository repository,
    ILogger<DeleteHandler> logger) : IRequestHandler<DeleteUserCommand, bool>
{
    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        logger.LogInformation("Start deleting {User} with {Id} in {DeleteHandler}.",
            typeof(User).Name, request.Id, typeof(DeleteHandler));

        return await repository.Delete(request.Id);
    }
}