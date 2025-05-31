using Npgsql;

namespace FlightPlanner.DatabaseConnection;

public static class FlightPlannerContext
{
    private static readonly IConfiguration _Configuration;

    static FlightPlannerContext()
    {
        _Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }
    
    public static NpgsqlConnection GetConnection()
    {
        var connectionString = _Configuration.GetConnectionString(name: "DefaultConnection");
        return new NpgsqlConnection(connectionString);
    }
}