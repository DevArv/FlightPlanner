using FlightPlanner.Enums;

namespace FlightPlanner.Models;

public class Planner
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
    
    public int NauticalMiles { get; set; } = 0;
    
    public decimal FuelRequired { get; set; } = 0;
    
    public int CruiseSpeedKnots { get; set; } = 0;
    
    public int AltitudeFeet { get; set; } = 0;
    
    public int ReserveFuel { get; set; } = 0;
    
    public int EmergencyFuel { get; set; } = 0;
    
    public int ArrivalRunwayElevation { get; set; } = 0;
    
    public int ArrivalRunwayMinimumAltitude { get; set; } = 0;
    
    public int ArrivalRunwayLength { get; set; } = 0;
    
    public decimal LocalizerFrequency { get; set; } = 0;
    
    public string LocalizerVectorName { get; set; } = "";
    
    public int LocalizarVectorAltitude { get; set; } = 0;
    
    public ApproachTypeEnum ApproachType { get; set; } = ApproachTypeEnum.DEFAULT;
    
    public FlightTypesEnum FlightType { get; set; } = FlightTypesEnum.DEFAULT;
    
    public decimal FlightEstimatedHourTime { get; set; } = 0;
    
    public int FlightEstimatedMinutesTime { get; set; } = 0;
    
    public int AverageFuelConsumption { get; set; } = 0;
    
    public int BasicFuel { get; set; } = 0;
    
    public decimal TotalFuel { get; set; } = 0;
}