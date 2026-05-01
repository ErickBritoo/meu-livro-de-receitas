using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using MyRecipeBook.Exceptions;
using Shouldly;
using WebApi.Test.InlineData;
using Xunit;

namespace WebApi.Test.User.Register;

public class RegisterUserTest : MyRecipeBookClassFixture
{
    private const string _routeMethod = "user";

    public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await DoPost(_routeMethod, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldNotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var response = await DoPost(_routeMethod, request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        errors.Count().ShouldBe(1);
        errors.Single().GetString().ShouldBe(expectedMessage);
    }
}