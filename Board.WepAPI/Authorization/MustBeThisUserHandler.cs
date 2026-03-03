using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Board.WepAPI.Authorization;

public class MustBeThisUserHandler : AuthorizationHandler<MustBeThisUserRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeThisUserRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            context.Fail();
            return Task.CompletedTask;
        }



        if (context.User.Identity?.IsAuthenticated == true)
        {
            var name = context.User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value
                ?? context.User.FindFirst(c => c.Type == "name")?.Value;

            var email = context.User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value
                ?? context.User.FindFirst(c => c.Type == "email")?.Value;


            var userIdFromToken = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var EmailFromToken = context.User.FindFirst(ClaimTypes.Email)?.Value;

            // Here you would typically check if the user ID in the token matches the user ID in the route or request
            // For example, you might extract the user ID from the claims and compare it to a route parameter
            // var userIdFromToken = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // var userIdFromRoute = ...; // Extract from route or request
            // if (userIdFromToken == userIdFromRoute)
            // {
            //     context.Succeed(requirement);
            // }
            context.Succeed(requirement); // For demonstration purposes, we are just succeeding if the user is authenticated
        }
        return Task.CompletedTask;
    }
}
