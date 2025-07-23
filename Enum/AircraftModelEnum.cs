using System.ComponentModel.DataAnnotations;

namespace FlightPlanner.Enum;

public enum AircraftModelEnum
{
    [Display(Name = "")]
    DEFAULT = 0,
    
    [Display(Name = "Cessna Citation Longitude")]
    CESSNA_CITATION_LONGITUDE = 10,
    
    [Display(Name = "Embraer E190")]
    EMBRAER_E190 = 20,
    
    [Display(Name = "Embraer E195")]
    EMBRAER_E195 = 30,
    
    [Display(Name = "Airbus A320 Neo")]
    AIRBUS_A320NEO = 40,
    
    [Display(Name = "Boeing 737 MAX")]
    BOEING_737_MAX = 50,
    
    [Display(Name = "Cessna S172 Skyhawk")]
    CESSNA_S172_SKYHAWK = 60,
    
    [Display(Name = "Diamond DA40 NG")]
    DIAMOND_DA40 = 70,
    
    [Display(Name = "Diamond DA62")]
    DIAMOND_DA62 = 80,
    
    [Display(Name = "Cessna C400 Corvalis TT")]
    CESSNA_C400 = 90
}