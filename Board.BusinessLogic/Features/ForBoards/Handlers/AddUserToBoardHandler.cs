using Board.BusinessLogic.Features.ForBoards.Commands;
using Board.Common.Exceptions;
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
            throw new BadRequestException(
                    $"The {typeof(User).Name} with email = {request.Email} doesn't exists in DB");
        }

        bool isUserMemberOfBoard = await boardRepository.CheckBoardMemberExistence(request.BoardId, request.Email);

        if (isUserMemberOfBoard)
        {
            throw new BadRequestException(
                $"The {typeof(User).Name} with email = {request.Email} is already a  member of the {typeof(DataAccess.Models.Board).Name} with Id = {request.BoardId}");
        }

        await boardRepository.AddBoardMember(request.BoardId, request.Email, request.Role);
    }
}