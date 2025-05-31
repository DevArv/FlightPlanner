using FlightPlanner.Enums;

namespace FlightPlanner.Models;

public class Planner : FlightSpecs
{
    public Guid ID { get; set; }
    
    public DateTime Date { get; set; }

    public string ICAODeparture { get; set; } = "";
    
    public string ICAOArrival { get; set; } = "";

    public string FullFlightName
    {
        get
        {
            return $"{ICAODeparture} => {ICAOArrival}";
        }
    }
    
    public string DepartureAirportName { get; set; } = "";
    
    public string ArrivalAirportName { get; set; } = "";

    public decimal BaroPressure { get; set; } = 0;
    
    public int TransitionAltitude { get; set; } = 0;
    
    public int DepartureRunway { get; set; } = 0;
    
    public int ArrivalRunway { get; set; } = 0;
    
    public int AltitudeFeet { get; set; } = 0;
    
    public int ArrivalRunwayElevation { get; set; } = 0;
    
    public int ArrivalRunwayMinimumAltitude { get; set; } = 0;
    
    public int ArrivalRunwayLength { get; set; } = 0;
    
    public decimal LocalizerFrequency { get; set; } = 0;
    
    public string LocalizerVectorName { get; set; } = "";
    
    public int LocalizarVectorAltitude { get; set; } = 0;
    
    public ApproachTypeEnum ApproachType { get; set; } = ApproachTypeEnum.DEFAULT;
    
    public FlightTypesEnum FlightType { get; set; } = FlightTypesEnum.DEFAULT;
}