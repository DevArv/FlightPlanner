using FlightPlanner.Enums;

namespace FlightPlanner.Models;

public class Planner
{
    public Guid ID { get; set; }
    
    public DateTime Date { get; set; }

    public string? ICAODeparture { get; set; } = "";
    
    public string? ICAOArrival { get; set; } = "";

    public string FullFlightName { get; set; } = "";
    
    public string? DepartureAirportName { get; set; } = "";

    public string? ArrivalAirportName { get; set; } = "";

    public decimal? BaroPressureDeparture { get; set; }
    
    public decimal? BaroPressureArrival { get; set; }
    
    public int? TransitionAltitudeDeparture { get; set; }
    
    public int? TransitionAltitudeArrival { get; set; }
    
    public int? DepartureRunway { get; set; }
    
    public int? ArrivalRunway { get; set; }
    
    public int? ArrivalRunwayElevation { get; set; }
    
    public int? ArrivalRunwayMinimumAltitude { get; set; }
    
    public int? ArrivalRunwayLength { get; set; }
    
    public decimal? LocalizerFrequency { get; set; }
    
    public string? LocalizerVectorName { get; set; } = "";
    
    public int? LocalizerVectorAltitude { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public ApproachTypeEnum ApproachType { get; set; } = ApproachTypeEnum.DEFAULT;
    
    public FlightTypesEnum FlightType { get; set; } = FlightTypesEnum.DEFAULT;
    
    public AircraftModelEnum AircraftModel { get; set; } = AircraftModelEnum.DEFAULT;
}