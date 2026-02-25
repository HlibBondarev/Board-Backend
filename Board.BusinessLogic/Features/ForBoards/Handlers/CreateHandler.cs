using Board.BusinessLogic.DTOs.Boards;
using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForBoards.Commands;
using Board.BusinessLogic.Features.ForIssue.Commands;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForBoards.Handlers;

public class CreateHandler(
    IBoardRepository repository,
    ILogger<CreateHandler> logger) : IRequestHandler<CreateBoardCommand, BoardCreateResponseDto>
{
    public async Task<BoardCreateResponseDto> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start creating {Board} in {CreateHandler}.",
            typeof(DataAccess.Models.Board).Name, typeof(CreateHandler));
        DataAccess.Models.Board board = request.ToModel();
        DataAccess.Models.Board result = await repository.CreateWithAdmin(board, request.UserId);
        logger.LogInformation("Successfully completed creating {Board} with {Id} in {BoardRepository}.",
            typeof(DataAccess.Models.Board).Name, result.Id, typeof(IIssueRepository));

        return result.ToDto();
    }
}