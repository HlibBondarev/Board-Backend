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
    public async Task<IEnumerable<UserResponseDto>> Handle(GetAllUsersQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing GetAllQuery for {User}s in GetAllHandler.", typeof(User).Name);
        var users = await repository.GetAll(SqlStatements.ForUsers.GetAll);
        logger.LogInformation("Successfully completed executing GetAllQuery for {User}s in EntityRepository.", typeof(User).Name);
        _ = users ?? throw new NotFoundException($"No users found");

        return users.ToDto();
    }
}