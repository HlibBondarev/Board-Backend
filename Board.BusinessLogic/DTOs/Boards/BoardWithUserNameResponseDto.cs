namespace Board.BusinessLogic.DTOs.Boards;

public record BoardWithUserNameResponseDto(
    string UserName,
    BoardResponseDto[] Boards);