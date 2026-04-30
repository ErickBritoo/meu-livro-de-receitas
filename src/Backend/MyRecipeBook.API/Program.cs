using MyRecipeBook.API.Filters;
using MyRecipeBook.API.Middlewares;
using MyRecipeBook.Application;
using Scalar.AspNetCore;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Application.Services.Mapster;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(); 

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddMvc(options => options.Filters.Add((typeof(ExceptionFilter))));

MapsterConfig.Register();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

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

MigrateDatabase();

await app.RunAsync();

return;

void MigrateDatabase()
{
    var connectionString = builder.Configuration.ConnectionString();

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    
    DatabaseMigration.Migrate(connectionString, serviceScope.ServiceProvider);
}

public partial class Program
{
    protected Program() {}
}