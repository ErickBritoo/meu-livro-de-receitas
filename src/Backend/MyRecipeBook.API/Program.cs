using MyRecipeBook.API.Filters;
using MyRecipeBook.API.Middlewares;
using MyRecipeBook.Application;
using Scalar.AspNetCore;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Application.Services.Mapster;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(); 

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAplication();

builder.Services.AddMvc(options => options.Filters.Add((typeof(ExceptionFilter))));

MapsterConfig.Register();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    app.MapScalarApiReference(options =>
    {
        options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
    });

    app.Map("", () => Results.Redirect("/scalar", permanent: true));
}

app.UseMiddleware<CultureMiddleware>();

app.MapControllers();

app.Run();