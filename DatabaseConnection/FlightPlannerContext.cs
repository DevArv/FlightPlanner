using FlightPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.DatabaseConnection;

public class FlightPlannerContext : DbContext
{
    private static readonly IConfiguration _Configuration;

    static FlightPlannerContext()
    {
        _Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .Build();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
    {
        var connectionString = _Configuration.GetConnectionString(name: "DefaultConnection");
        OptionsBuilder.UseNpgsql(connectionString);
    }
    
    public DbSet<Planner> FlightPlanner { get; set; }
    public DbSet<FlightSpecs> FlightSpecs { get; set; }
}