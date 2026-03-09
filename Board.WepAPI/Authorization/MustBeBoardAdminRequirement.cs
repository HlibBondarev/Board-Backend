using Microsoft.AspNetCore.Authorization;

namespace Board.WepAPI.Authorization;

public class MustBeBoardAdminRequirement : IAuthorizationRequirement
{
    public MustBeBoardAdminRequirement()
    {
    }
}