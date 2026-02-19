using Board.BusinessLogic.DTOs.Users;
using Board.BusinessLogic.Features.ForUser.Queries;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForUser.Handlers;

public class GetByIdHandler(
    IUserRepository repository,
    ILogger<GetByIdHandler> logger) : IRequestHandler<GetUserByIdQuery, UserResponseDto>
{
    public async Task<UserResponseDto> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing GetByIdQuery for {User} with {Id} in {GetByIdHandler}.",
            typeof(User).Name, request.Id, typeof(GetByIdHandler));
        var user = await repository.GetById(request.Id);
        logger.LogInformation("Successfully completed executing GetByIdQuery for {User} with {Id} in {UserRepository}.",
            typeof(User).Name, user.Id, typeof(IUserRepository));
        _ = user ?? throw new NotFoundException($"{typeof(User).Name} with Id = {request.Id} not found");

        return user.ToDto();
    }
}