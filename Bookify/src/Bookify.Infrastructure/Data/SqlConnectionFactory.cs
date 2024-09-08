namespace Bookify.Infrastructure;

using System.Data;
using Npgsql;

internal sealed class SqlConnectionFactory: ISqlConnectionFactory
{
    private readonly string connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        return connection;
    }
}