using System.Net.Http.Json;
using Xunit;

namespace WebApi.Test;

public class MyRecipeBookClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public MyRecipeBookClassFixture(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(string routeMethod, object request, string culture = "en")
    {
        ChangeRequestCulture(culture);

        return await _httpClient.PostAsJsonAsync(routeMethod, request);
    }
    
    private void ChangeRequestCulture(string culture)
    {
        var acceptLanguageHeader = "Accept-Language";
        
        if (_httpClient.DefaultRequestHeaders.Contains(acceptLanguageHeader))
            _httpClient.DefaultRequestHeaders.Remove(acceptLanguageHeader);
        
        _httpClient.DefaultRequestHeaders.Add(acceptLanguageHeader, culture);

    }
}
