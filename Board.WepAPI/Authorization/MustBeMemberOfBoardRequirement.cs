using Microsoft.AspNetCore.Authorization;

namespace Board.WepAPI.Authorization;

public class MustBeMemberOfBoardRequirement : IAuthorizationRequirement
{
    public MustBeMemberOfBoardRequirement()
    {
    }
}
