using Board.BusinessLogic.Features.ForUser.Commands;

namespace Board.BusinessLogic.Services.Api;

public interface ICurrentUser
{
    Task<CreateUserCommand?> GetUserPropertiesFromClaims(string authorizationHeader);
}
