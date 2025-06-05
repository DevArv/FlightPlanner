using FlightPlanner.Enums;

namespace FlightPlanner.ViewModels;

public class FlightPlannerSimpleViewModel
{
    public Guid ID { get; set; }
    public DateTime Date { get; set; }
    public string ICAODeparture { get; set; }
    public string DepartureAirportName { get; set; }
    public decimal BaroPressureDeparture { get; set; }
    public int TransitionAltitudeDeparture { get; set; }
    public int DepartureRunway { get; set; }
    public string ICAOArrival { get; set; }
    public string ArrivalAirportName { get; set; }
    public int ArrivalRunway { get; set; }
    public decimal BaroPressureArrival { get; set; }
    public int TransitionAltitudeArrival { get; set; }
    public int ArrivalRunwayElevation { get; set; }
    public int ArrivalRunwayMinimumAltitude { get; set; }
    public int ArrivalRunwayLength { get; set; }
    public int AltitudeFeet { get; set; }
    public int LocalizerVectorAltitude { get; set; }
    public decimal LocalizerFrequency { get; set; }
    public string LocalizerVectorName { get; set; }
    public ApproachTypeEnum ApproachType { get; set; }
    public AircraftModelEnum AircraftModel { get; set; }
    public FlightTypesEnum FlightType { get; set; }
    public FlightSpecsViewModel FlightSpecs { get; set; } = new();
}

public class FlightSpecsViewModel
{
    public int? NauticalMiles { get; set; }
    public int? CruiseSpeedKnots { get; set; }
}

public class FlightSpecsDetailsViewModel : FlightSpecsViewModel
{
    public decimal FlightEstimatedHourTime { get; set; }
    public int FlightEstimatedMinutesTime { get; set; }
}

public class FlightPlannerDetailsViewModel : FlightPlannerSimpleViewModel
{
    public string FullFlightName { get; set; } = "";
    public FlightSpecsDetailsViewModel FlightSpecs { get; set; } = new();
}