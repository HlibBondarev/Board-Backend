using Board.BusinessLogic.DTOs.Users;
using Board.BusinessLogic.Features.ForUser.Queries;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForUser.Handlers;

public class GetAllHandler(IEntityRepositoryBase<string, User> repository,
    ILogger<GetAllHandler> logger) : IRequestHandler<GetAllUsersQuery, IEnumerable<UserResponseDto>>
{
    private readonly IEntityRepositoryBase<string, User> _repository = repository;
    private readonly ILogger<GetAllHandler> _logger = logger;

    public async Task<IEnumerable<UserResponseDto>> Handle(GetAllUsersQuery request, CancellationToken ct)
    {
        _logger.LogInformation("Start executing GetAllQuery for {User}s in UpdateHandler.", typeof(User).Name);
        var users = await _repository.GetAll(SqlStatements.ForUsers.GetAll);
        _ = users ?? throw new NotFoundException($"No users found");
        _logger.LogInformation("Successfully completed executing GetAllQuery for {User}s in EntityRepository.", typeof(User).Name);

        return users.ToDto();
    }
}