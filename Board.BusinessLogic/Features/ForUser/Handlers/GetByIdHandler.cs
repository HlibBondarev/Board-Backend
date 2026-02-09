using Board.BusinessLogic.DTOs.Users;
using Board.BusinessLogic.Features.ForUser.Queries;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForUser.Handlers;

public class GetByIdHandler(IEntityRepositoryBase<int, User> repository,
    ILogger<DeleteHandler> logger) : IRequestHandler<GetByIdQuery, UserResponseDto>
{
    private readonly IEntityRepositoryBase<int, User> _repository = repository;
    private readonly ILogger<DeleteHandler> _logger = logger;

    public async Task<UserResponseDto> Handle(GetByIdQuery request, CancellationToken ct)
    {
        _logger.LogInformation("Start executing GetByIdQuery for User with {Id} in GetByIdHandler.", request.Id);
        var user = await _repository.GetById(request.Id, SqlStatements.ForUsers.GetById);
        _ = user ?? throw new NotFoundException($"User with Id = {request.Id} not found");
        _logger.LogInformation("Successfully completed executing GetByIdQuery for User with {Id} in EntityRepository.", user.Id);

        return user.ToDto();
    }
}