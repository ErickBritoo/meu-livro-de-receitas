using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace MyRecipeBook.Infrastructure.Migrations;

public static class DatabaseMigration
{
    public static void Migrate(string connectionString, IServiceProvider serviceProvider)
    {
        EnsureDatabaseCreated(connectionString);
        MigrationDatabase(serviceProvider);
    }

    private static void EnsureDatabaseCreated(string connectionString)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);

        var databaseName = connectionStringBuilder.Database;

        connectionStringBuilder.Remove("Database");
        
        using var databaseConnection = new MySqlConnection(connectionStringBuilder.ConnectionString);
        
        var parameters = new DynamicParameters();
        parameters.Add("name", databaseName);

        var records = databaseConnection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @name", parameters);

        if (!records.Any())
        {
            databaseConnection.Execute($"CREATE DATABASE {databaseName}");
        }
    }

    private static void MigrationDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        
        runner.ListMigrations();
        
        runner.MigrateUp();
    }
}