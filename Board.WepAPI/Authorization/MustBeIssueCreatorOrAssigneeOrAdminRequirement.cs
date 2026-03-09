using Microsoft.AspNetCore.Authorization;

namespace Board.WepAPI.Authorization;

public class MustBeIssueCreatorOrAssigneeOrAdminRequirement : IAuthorizationRequirement
{
    public MustBeIssueCreatorOrAssigneeOrAdminRequirement()
    {
    }
}