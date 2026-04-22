using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

public abstract class VersionBase : ForwardOnlyMigration
{
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string tableName)
    {
        return Create.Table(tableName).WithColumn("ID").AsInt64().PrimaryKey().Identity()
            .WithColumn("Active").AsBoolean().NotNullable()
            .WithColumn("CreatedOn").AsDateTime().NotNullable();
    }
}