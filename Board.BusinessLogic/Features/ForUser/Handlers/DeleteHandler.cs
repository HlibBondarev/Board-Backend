using Board.BusinessLogic.Features.ForUser.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForUser.Handlers;

public class DeleteHandler(IEntityRepositoryBase<int, User> repository,
    ILogger<DeleteHandler> logger) : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IEntityRepositoryBase<int, User> _repository = repository;
    private readonly ILogger<DeleteHandler> _logger = logger;

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Start deleting {User} with {Id} in DeleteHandler.", typeof(User).Name, request.Id);

        return await _repository.Delete(request.Id, SqlStatements.ForUsers.Delete);
    }
}