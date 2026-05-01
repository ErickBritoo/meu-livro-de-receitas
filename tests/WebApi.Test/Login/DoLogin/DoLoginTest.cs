using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using MyRecipeBook.Communication.Requests;
using Shouldly;
using Xunit;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest : MyRecipeBookClassFixture
{
    private readonly string _routeMethod = "login";    
    private readonly string _email;
    private readonly string _password;
    private readonly string _name;

    public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _email = factory.GetEmail();
        _password = factory.GetPassword();
        _name = factory.GetName();
    }

    [Fact]
    public async Task Sucess()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        var response = await DoPost(_routeMethod, request);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldNotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_name);
        
    }
}