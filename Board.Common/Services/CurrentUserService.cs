using Board.Common.Services.Api;
using Board.Common.Services.DTOs;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Text.Json;

namespace Board.Common.Services;

public class CurrentUserService(HttpClient httpClient) : ICurrentUserService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<UserFromClaimsDto> GetUserPropertiesFromClaims(string authorizationHeader)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "userinfo");

        request.Headers.Add("Authorization", authorizationHeader);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new UnauthorizedAccessException("The user is not authenticated.");
        }

        return (await response.Content.ReadFromJsonAsync<UserFromClaimsDto>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })) ??
            throw new AuthenticationException(
                $"Can not get user's claims from Context.");
    }
}
