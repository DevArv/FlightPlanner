using System.ComponentModel.DataAnnotations;

namespace FlightPlanner.Enum;

public enum FlightTypesEnum
{
    [Display(Name = "")]
    DEFAULT = 0,
    
    [Display(Name = "Vuelo Corto ( < 350 NM )")]
    SHORT_FLIGHT = 10,
    
    [Display(Name = "Vuelo Mediano Alcance ( 351 - 1499 NM )")]
    NORMAL_FLIGHT = 20,
    
    [Display(Name = "Vuelo Alta Altitud ( > 1500 NM )")]
    HIGH_ALTITUDE_FLIGHT = 30,
}
