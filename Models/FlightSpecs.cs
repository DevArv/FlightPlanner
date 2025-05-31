using System.ComponentModel.DataAnnotations.Schema;

namespace FlightPlanner.Models;

public class FlightSpecs
{
    public int NauticalMiles { get; set; } = 0;
    
    public decimal FuelRequired { get; set; } = 0;
    
    public decimal FlightEstimatedHourTime { get; set; } = 0;
    
    public int FlightEstimatedMinutesTime { get; set; } = 0;
    
    public int CruiseSpeedKnots { get; set; } = 0;
    
    public int AverageFuelConsumption { get; set; } = 0;
    
    public int BasicFuel { get; set; } = 0;
    
    public decimal TotalFuel { get; set; } = 0;
    
    public int ReserveFuel { get; set; } = 0;
    
    public int EmergencyFuel { get; set; } = 0;
    
    [NotMapped]
    public decimal TotalFuelGal { get; set; } = 0;
    
    [NotMapped]
    public decimal ReserveFuelGal { get; set; } = 0;
    
    [NotMapped]
    public decimal EmergencyFuelGal { get; set; } = 0;
}