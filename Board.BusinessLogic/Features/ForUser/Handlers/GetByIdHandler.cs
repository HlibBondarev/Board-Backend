using Board.BusinessLogic.DTOs.Users;
using Board.BusinessLogic.Features.ForUser.Queries;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForUser.Handlers;

public class GetByIdHandler(IEntityRepositoryBase<string, User> repository,
    ILogger<GetByIdHandler> logger) : IRequestHandler<GetUserByIdQuery, UserResponseDto>
{
    public async Task<UserResponseDto> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing GetByIdQuery for {User} with {Id} in GetByIdHandler.",
            typeof(User).Name, request.Id);
        var user = await repository.GetById(request.Id, SqlStatements.ForUsers.GetById);
        _ = user ?? throw new NotFoundException($"{typeof(User).Name} with Id = {request.Id} not found");
        logger.LogInformation("Successfully completed executing GetByIdQuery for {User} with {Id} in EntityRepository.",
            typeof(User).Name, user.Id);

        return user.ToDto();
    }
}