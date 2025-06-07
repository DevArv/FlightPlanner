using System.ComponentModel.DataAnnotations.Schema;

namespace FlightPlanner.Models;

public class FlightSpecs
{
    public Guid ID { get; set; }
    
    public Guid PlannerID { get; set; }
    
    [ForeignKey("PlannerID")]
    public Planner? Planner { get; set; }
    
    public int? NauticalMiles { get; set; }
    
    public int? CruiseSpeedKnots { get; set; }
    
    public int AverageFuelConsumption { get; set; }
    
    public decimal BasicFuel { get; set; }
    
    public int ReserveFuel { get; set; }
    
    public int EmergencyFuel { get; set; }
    
    public decimal TotalFuel { get; set; }
    
    public decimal TotalFuelGal { get; set; }
    
    public decimal ReserveFuelGal { get; set; }
    
    public decimal EmergencyFuelGal { get; set; }
    
    public decimal FlightEstimatedHourTime { get; set; }
    
    public int FlightEstimatedMinutesTime { get; set; }
}