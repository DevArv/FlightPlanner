using System.ComponentModel.DataAnnotations.Schema;

namespace FlightPlanner.Models;

public class FlightSpecs
{
    public Guid ID { get; set; }
    
    public Guid PlannerID { get; set; }
    
    [ForeignKey("PlannerID")]
    public Planner? Planner { get; set; }
    
    public int NauticalMiles { get; set; }
    
    public int? CruiseSpeedKnots { get; set; }
    
    public int AverageFuelConsumption { get; set; }
    
    public decimal BasicFuel { get; set; }
    
    public int ReserveFuel { get; set; }
    
    public decimal TotalFuel { get; set; }
    
    public decimal TotalFuelGal { get; set; }
    
    public decimal ReserveFuelGal { get; set; }
    
    public decimal FlightEstimatedHourTime { get; set; }
    
    public int FlightEstimatedMinutesTime { get; set; }
    
    public int AltitudeFeet { get; set; }
    
    public int AscentRateFeetPerMinute { get; set; }
    
    public int DescentRateFeetPerMinute { get; set; }
    
    public int FinalApproachSpeed { get; set; }
    
    public int DescentVerticalSpeed { get; set; }
}