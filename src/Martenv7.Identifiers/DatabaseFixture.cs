namespace Martenv7.Identifiers;

using Marten;
using Microsoft.Extensions.Configuration;
using Npgsql;

public class MartenFixture : IDisposable
{
    private readonly string _schema = $"test{Guid.NewGuid().ToString().Replace("-", string.Empty)}";
    public IDocumentStore Store { get; private set; }
    
    public MartenFixture()
    {
        CreateSchema();
        
        Store = DocumentStore.For(options =>
        {
            options.Connection(GetConnectionString());
            options.DatabaseSchemaName = _schema;

            options.Schema.For<EntityWithoutNaturalId>()
                .Identity(entity => entity.OtherId);
        });
    }

    public void Dispose()
    {
        DropSchema();
    }
    
    private void CreateSchema()
    {
        using var connection = new NpgsqlConnection(GetConnectionString());
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = $"CREATE SCHEMA IF NOT EXISTS {_schema}";
        command.ExecuteNonQuery();
        connection.Close();
    }

    private void DropSchema()
    {
        using var connection = new NpgsqlConnection(GetConnectionString());
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = $"DROP SCHEMA IF EXISTS {_schema} CASCADE";
        command.ExecuteNonQuery();
        connection.Close();
    }
    
    private string? _connectionString;
    private string GetConnectionString()
    {
        if (_connectionString != null)
            return _connectionString;
        
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        _connectionString = config.GetConnectionString("Marten");
        
        if(_connectionString is null)
            throw new Exception("ConnectionString unavailable");

        return _connectionString;
    }
}

[CollectionDefinition("Marten collection")]
public class DatabaseCollection : ICollectionFixture<MartenFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}