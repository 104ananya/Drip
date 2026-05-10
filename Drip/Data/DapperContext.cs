using Npgsql;           // PostgreSQL driver — gives us NpgsqlConnection
using System.Data;      // Gives us IDbConnection — standard interface Dapper uses

namespace Drip.Data;

public class DapperContext          // Use of this Class -> Create Databse connection when asked for it, using the connection string from appsettings.json
{
    private readonly string _connectionString; // Stored once, never changes
    // _prefix is naming convention for private fields

    // This is the Constructor — IConfiguration is injected by .NET's DI, reads from appsettings.json
    public DapperContext(IConfiguration configuration)
    {
        // Reads "ConnectionStrings:DefaultConnection" from appsettings.json
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // ! is null-forgiving operator — tells compiler "I know this won't be null, trust me"
    }

    // Creates a new PostgreSQL connection — called every time we need to run a query
    public IDbConnection CreateConnection()
        => new NpgsqlConnection(_connectionString);

    /** This is a  method / function that returns a new database connection. 
     It uses the NpgsqlConnection class, which is specific to PostgreSQL, 
     and it passes in the connection string we stored from the constructor. 
     The return type is IDbConnection, which is an interface that Dapper works with, 
     allowing us to use this method to get a connection whenever we need to run a query against the database.  **/
}
