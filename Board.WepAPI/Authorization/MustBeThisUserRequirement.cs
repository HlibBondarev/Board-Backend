using Microsoft.AspNetCore.Authorization;

namespace Board.WepAPI.Authorization;

public class MustBeThisUserRequirement : IAuthorizationRequirement
{
    public MustBeThisUserRequirement()
    {
    }
}
