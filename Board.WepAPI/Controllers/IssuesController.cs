using Board.BusinessLogic.DTOs.Columns;
using Board.BusinessLogic.DTOs.Issues;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Board.WepAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IssuesController(IMediator mediator, ILogger<ColumnsController> logger) : ControllerBase
{
    [HttpPatch]
    [Route("{id}/move")]
    public async Task<ActionResult<ColumnResponseDto>> GetIssuePositionAfterMove(int id, [FromBody] MoveIssueRequestDto dto)
    {

        return null;
    }
}
