using System.ComponentModel.DataAnnotations.Schema;

namespace FlightPlanner.Models;

//TODO: Probar si esto es realmente necesario..
[Table("FlightSpecs")]
public class FlightSpecs
{
    public Guid ID { get; set; }
    
    public Guid PlannerID { get; set; }
    
    [ForeignKey("PlannerID")]
    public Planner Planner { get; set; }
    
    public int? NauticalMiles { get; set; }
    
    //public decimal FuelRequired { get; set; }
    
    public int? CruiseSpeedKnots { get; set; }
    
    //public int AverageFuelConsumption { get; set; }
    
    //public int BasicFuel { get; set; }
    
    //public decimal TotalFuel { get; set; }
    
    //public int ReserveFuel { get; set; }
    
    //public int EmergencyFuel { get; set; }
    
    //[NotMapped]
    //public decimal TotalFuelGal { get; set; }
    
    //[NotMapped]
    //public decimal ReserveFuelGal { get; set; }
    
    //[NotMapped]
    //public decimal EmergencyFuelGal { get; set; }
    
    public decimal FlightEstimatedHourTime { get; set; }
    
    public int FlightEstimatedMinutesTime { get; set; }
}