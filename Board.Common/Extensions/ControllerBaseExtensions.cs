using Board.Common.Services.Api;
using Board.Common.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace Board.Common.Extensions;

public static class ControllerBaseExtensions
{

    public static async Task<string> GetUserId(this ControllerBase controllerBase, ICurrentUserService currentUserService)
    {
        var userFromClaims = await GetUserClaims(controllerBase, currentUserService);

        return userFromClaims.Id ?? throw new AuthenticationException(
            $"Can not get user's claim {nameof(IdentityResourceClaimsTypes.Sub)} from Context.");
    }

    public static async Task<UserFromClaimsDto> GetUserClaims(this ControllerBase controllerBase, ICurrentUserService currentUserService)
    {
        var authorizationHeader = controllerBase.Request.Headers["Authorization"];
        var token = authorizationHeader.FirstOrDefault() ??
            throw new InvalidOperationException("The request headers don't have the Authorization header.");

        var userFromClaims = (await currentUserService.GetUserPropertiesFromClaims(token)) ??
            throw new AuthenticationException("Can not get user's claims from Context.");

        return userFromClaims;
    }
}