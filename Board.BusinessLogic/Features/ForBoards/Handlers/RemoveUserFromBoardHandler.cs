using Board.BusinessLogic.Features.ForBoards.Commands;
using Board.Common.Exceptions;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForBoards.Handlers;

public class RemoveUserFromBoardHandler(
    IBoardRepository boardRepository,
    IUserRepository userRepository,
    ILogger<RemoveUserFromBoardHandler> logger) : IRequestHandler<RemoveUserFromBoardCommand>
{
    public async Task Handle(RemoveUserFromBoardCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start removing {User} from {Board} in {RemoveUserFromBoardHandler}.",
            typeof(User).Name,
            typeof(DataAccess.Models.Board).Name,
            typeof(RemoveUserFromBoardHandler).Name);

        bool isUserExists = await userRepository.UserEmailIsExists(request.Email);

        if (!isUserExists)
        {
            throw new BadRequestException(
                    $"The {typeof(User).Name} with email = {request.Email} doesn't exists in DB");
        }

        bool isUserMemberOfBoard = await boardRepository.CheckBoardMembershipByEmail(request.BoardId, request.Email);

        if (!isUserMemberOfBoard)
        {
            throw new BadRequestException(
                $"The {typeof(User).Name} with email = {request.Email} is not a  member of the {typeof(DataAccess.Models.Board).Name} with Id = {request.BoardId}");
        }

        await boardRepository.RemoveBoardMember(request.BoardId, request.Email);
    }
}