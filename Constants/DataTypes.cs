namespace FlightPlanner.Constants;

public class PartialRoutes
{
    public static string SHARED_TEMPLATE = "Shared/EditorTemplates/";
}

public abstract class DataTypes : PartialRoutes
{
    public static string ICAO = nameof(ICAO);
    public static string AirportName = nameof(AirportName);
    public static string Runway = nameof(Runway);
    public static string BaroPressure = nameof(BaroPressure);
    public static string TransitionAltitude = nameof(TransitionAltitude);
}