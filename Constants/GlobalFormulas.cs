namespace FlightPlanner.Constants;

public static class GlobalFormulas
{
    //CESSNA CITATION LONGITUDE - FUEL CONSUMPTION
    public static int CESSNACL_HA_AVERAGE_FUEL = 1700;
    public static int CESSNACL_HA_RESERVE_FUEL = 1200;
    public static int CESSNACL_HA_EMERGENCY_FUEL = 850;
    
    public static int CESSNACL_MA_AVERAGE_FUEL = 2000;
    public static int CESSNACL_MA_RESERVE_FUEL = 1500;
    public static int CESSNACL_MA_EMERGENCY_FUEL = 1000;
    
    public static int CESSNACL_LA_AVERAGE_FUEL = 2200;
    public static int CESSNACL_LA_RESERVE_FUEL = 1650;
    public static int CESSNACL_LA_EMERGENCY_FUEL = 1100;
    
    //DENSITY FUEL
    public static decimal DENSITY_FUEL_GAL = 6.695m; // 6.7 lbs per gallon
    
    //CESSNA CITATION LONGITUDE - CRUISE ALTITUDE
    public static int CESSNACL_HA_CRUISE_ALTITUDE = 45000;
    public static int CESSNACL_MA_CRUISE_ALTITUDE = 41000;
    public static int CESSNACL_LA_CRUISE_ALTITUDE = 34000;
    
    //CESSNA CITATION LONGITUDE - CRUISE SPEED
    public static int CESSNACL_HA_CRUISE_SPEED = 445;
    public static int CESSNACL_MA_CRUISE_SPEED = 475;
    public static int CESSNACL_LA_CRUISE_SPEED = 440;
}