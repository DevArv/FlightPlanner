namespace FlightPlanner.Models;

public class FlightKnots
{
    // El peso define las velocidades 
    public int CurrentWeight { get; set; } = 0;
    
    // (VR) Velocidad para levantamiento
    public int RotateSpeed { get; set; } = 0;
    
    // (VX) Velocidad para mejor ángulo de ascenso
    public int BestAngleOfClimbSpeed { get; set; } = 0;
    
    // (VY) Velocidad para mejor tasa de ascenso
    public int BestRateOfClimbSpeed { get; set; } = 0;
    
    // (VT) Velocidad de crucero
    public int CruiseSpeed { get; set; } = 0;
    
    // (VREF) Velocidad de referencia para aterrizaje final.
    public int ReferenceLandingSpeed { get; set; } = 0;
    
    // (VGA) Velocidad para abortar aterrizaje
    public int GoAroundSpeed { get; set; } = 0;
    
    // (VT descenso) Velocidad de descenso
    public int RecommendedDescentSpeed { get; set; } = 0;
}