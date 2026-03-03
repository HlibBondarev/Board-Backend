using Board.Common.Services.DTOs;

namespace Board.Common.Services.Api;

public interface ICurrentUserService
{
    Task<UserFromClaimsDto> GetUserPropertiesFromClaims(string authorizationHeader);
}
