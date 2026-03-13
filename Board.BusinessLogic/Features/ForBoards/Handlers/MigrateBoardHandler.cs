using Board.BusinessLogic.Features.ForBoards.Commands;
using Board.DataAccess.Repository.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.BusinessLogic.Features.ForBoards.Handlers;

public class MigrateBoardHandler(
    IBoardRepository boardRepository,
    ILogger<MigrateBoardHandler> logger) : IRequestHandler<MigrateBoardCommand, long>
{
    public async Task<long> Handle(MigrateBoardCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start migration of demo-{Board} in {MigrateBoardHandler}.",
            typeof(DataAccess.Models.Board).Name, typeof(MigrateBoardHandler).Name);

        DataAccess.Models.Board board = request.ToModel();

        long tempIdCounter = 1;
        int position = 0;

        foreach (var col in board.Columns)
        {
            long currentTempId = tempIdCounter++;
            col.Id = currentTempId;
            col.Position = position++;

            long positionInColumn = 0;

            foreach (var issue in col.Issues)
            {
                issue.AssigneeId = request.UserId;
                issue.CreatorId = request.UserId;
                issue.ColumnId = currentTempId;
                issue.PositionInColumn = positionInColumn++;
            }
        }

        long boardId = await boardRepository.MigrateBoard(board, request.UserId);
        logger.LogInformation("Successfully completed migration of demo-{Board} with {Id} in {BoardRepository}.",
            typeof(DataAccess.Models.Board).Name, boardId, typeof(IIssueRepository).Name);

        return boardId;
    }
}