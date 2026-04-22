using System.Globalization;

namespace MyRecipeBook.API.Middlewares;

public class CultureMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    
    public async Task InvokeAsync(HttpContext context)
    {
        var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        var requestCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault()?.Split(',')[0];

        var culture = new CultureInfo("en");
        
        if (!string.IsNullOrWhiteSpace(requestCulture) && supportedCultures.Any(c => c.Name.Equals(requestCulture)))
        {
            culture = new CultureInfo(requestCulture);
        }
        
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        await _next.Invoke(context);
    }
}