using System.ComponentModel.DataAnnotations;

namespace FlightPlanner.Enum;

public enum ApproachTypeEnum
{
    [Display(Name = "")]
    DEFAULT = 0,
    
    [Display(Name = "ILS (Instrument Landing System)")]
    ILS = 10,
    
    [Display(Name = "RNAV (Area Navigation)")]
    RNAV = 20,
    
    [Display(Name = "VOR Approach")]
    VOR = 30,
    
    [Display(Name = "Visual Approach")]
    VISUAL = 40,
}
