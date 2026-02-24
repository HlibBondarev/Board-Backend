using Board.BusinessLogic.DTOs.Boards;
using Board.BusinessLogic.DTOs.Issues;
using Board.BusinessLogic.Features.ForBoards.Queries;
using Board.DataAccess.Models;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForBoards.Handlers;

public class GetByUserIdHandler(
    IBoardRepository repository,
    ILogger<GetByUserIdHandler> logger) : IRequestHandler<GetBoardsByUserIdQuery, IEnumerable<BoardResponseDto>>
{
    public async Task<IEnumerable<BoardResponseDto>> Handle(GetBoardsByUserIdQuery request, CancellationToken ct)
    {
        logger.LogInformation("Start executing GetByUserIdQuery for {Board} with {Id} in {GetByUserIdHandler}.",
            typeof(DataAccess.Models.Board).Name, request.Id, typeof(GetByUserIdHandler));
        var boards = await repository.GetByUserId(request.Id);
        logger.LogInformation("Successfully completed executing GetByIdUserQuery for {User} with {Id} in {BoardRepository}.",
            typeof(DataAccess.Models.Board).Name, request.Id, typeof(IBoardRepository));

        if (boards is null)
        {
            logger.LogInformation("{typeof(DataAccess.Models.Board).Name}s for {typeof(User).Name} with Id = {request.Id} not found",
            typeof(DataAccess.Models.Board).Name, typeof(User).Name, request.Id);
            return [];
        }

        return boards.ToDto();
    }
}