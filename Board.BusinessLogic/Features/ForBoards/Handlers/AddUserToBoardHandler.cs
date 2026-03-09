using Board.BusinessLogic.Features.ForBoards.Commands;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForBoards.Handlers;

public class AddUserToBoardHandler(
    IBoardRepository boardRepository,
    IUserRepository userRepository,
    ILogger<CreateHandler> logger) : IRequestHandler<AddUserToBoardCommand>
{
    public async Task Handle(AddUserToBoardCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start adding {User} to {Board} in {AddUserToBoardHandler}.",
            typeof(User).Name,
            typeof(DataAccess.Models.Board).Name,
            typeof(AddUserToBoardHandler).Name);

        bool isUserExists = await userRepository.UserEmailIsExists(request.Email);

        if (!isUserExists)
        {
            throw new InvalidOperationException(
                    $"The {typeof(User).Name} with email = {request.Email} doesn't exists in DB");
        }

        bool isUserMemberOfBoard = await boardRepository.CheckBoardMembershipWithRoleByEmail(request.BoardId, request.Email, request.Role);

        if (isUserMemberOfBoard)
        {
            throw new InvalidOperationException(
                $"The {typeof(User).Name} with email = {request.Email} has already had the role - '{request.Role}' in the {typeof(DataAccess.Models.Board).Name} with Id = {request.BoardId}");
        }

        await boardRepository.AddBoardMember(request.BoardId, request.Email, request.Role);
    }
}