using Microsoft.AspNetCore.Authorization;

namespace Board.WepAPI.Authorization;

public class MustBeIssueAssigneeOrAdminOrIssueAssigneeIsNullRequirement : IAuthorizationRequirement
{
    public MustBeIssueAssigneeOrAdminOrIssueAssigneeIsNullRequirement()
    {
    }
}
