using Board.BusinessLogic.Features.ForUser.Commands;
using Board.BusinessLogic.Services.Api;
using System.Net.Http.Json;
using System.Text.Json;

namespace Board.BusinessLogic.Services;

public class CurrentUser : ICurrentUser
{
    private readonly HttpClient _httpClient;

    public CurrentUser(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<CreateUserCommand?> GetUserPropertiesFromClaims(string authorizationHeader)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "userinfo");

        request.Headers.Add("Authorization", authorizationHeader);

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CreateUserCommand>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        return null;
    }
    //public class CurrentUser(IConfiguration configuration, IHttpClientFactory clientFactory) : ICurrentUser
    //{
    //    private readonly string _auth0UserInfo = $"{configuration["Auth0:Authority"]}userinfo";
    //    private readonly IHttpClientFactory _clientFactory = clientFactory;

    //public async Task<CreateUserCommand?> GetUserPropertiesFromClaims(string authorizationHeader)
    //{
    //    var request = new HttpRequestMessage(
    //      HttpMethod.Get,
    //      _auth0UserInfo);

    //    request.Headers.Add(
    //      "Authorization",
    //      authorizationHeader);

    //    var client = _clientFactory.CreateClient();
    //    var response = await client.SendAsync(request);
    //    if (response.IsSuccessStatusCode)
    //    {
    //        var jsonContent =
    //          await response.Content.ReadAsStringAsync();
    //        var user =
    //          JsonSerializer.Deserialize<CreateUserCommand>(
    //            jsonContent,
    //            new JsonSerializerOptions
    //            {
    //                PropertyNameCaseInsensitive = true
    //            });
    //        return user;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}
}
