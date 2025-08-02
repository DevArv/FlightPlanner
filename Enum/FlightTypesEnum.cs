using System.ComponentModel.DataAnnotations;

namespace FlightPlanner.Enum;

public enum FlightTypesEnum
{
    [Display(Name = "")]
    DEFAULT = 0,
    
    [Display(Name = "Vuelo Corto Alcance")]
    SHORT_FLIGHT = 10,
    
    [Display(Name = "Vuelo Mediano Alcance")]
    NORMAL_FLIGHT = 20,
    
    [Display(Name = "Vuelo Largo Alcance")]
    HIGH_ALTITUDE_FLIGHT = 30,
}