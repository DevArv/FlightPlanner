using FlightPlanner.Repositories;
using FlightPlanner.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlightPlanner.Pages.Flight;

[IgnoreAntiforgeryToken]
public class Edit : PageModel
{
    public FlightPlannerSimpleViewModel Record { get; set; } = new();

    public async Task<IActionResult> OnGet(Guid ID)
    {
        var repo = new PlannerRepository();
        try
        {
            var planner = await repo.GetFlightPlanAsync(ID: ID);
            Record = new FlightPlannerSimpleViewModel
            {
                ID = planner.ID,
                Date = planner.Date,
                ICAODeparture = planner.ICAODeparture,
                DepartureAirportName = planner.DepartureAirportName,
                BaroPressureDeparture = planner.BaroPressureDeparture,
                TransitionAltitudeDeparture = planner.TransitionAltitudeDeparture,
                DepartureRunway = planner.DepartureRunway,
                ICAOArrival = planner.ICAOArrival,
                ArrivalAirportName = planner.ArrivalAirportName,
                BaroPressureArrival = planner.BaroPressureArrival,
                TransitionAltitudeArrival = planner.TransitionAltitudeArrival,
                ArrivalRunway = planner.ArrivalRunway,
                ArrivalRunwayElevation = planner.ArrivalRunwayElevation,
                ArrivalRunwayMinimumAltitude = planner.ArrivalRunwayMinimumAltitude,
                LocalizerFrequency = planner.LocalizerFrequency,
                LocalizerVectorName = planner.LocalizerVectorName,
                ApproachType = planner.ApproachType,
                AircraftModel = planner.AircraftModel,
                FlightType = planner.FlightType,
                ArrivalRunwayLength = planner.ArrivalRunwayLength,
                LocalizerVectorAltitude = planner.LocalizerVectorAltitude,
                FlightSpecs = new FlightSpecsViewModel
                {
                    NauticalMiles = planner.FlightSpecs.NauticalMiles,
                    CruiseSpeedKnots = planner.FlightSpecs.CruiseSpeedKnots
                }
            };
            return Page();
        }
        catch (ApplicationException aex)
        {
            return RedirectToPage("/Index", new { error = aex.Message });
        }
    }
    
    public async Task<IActionResult> OnPostSaveAsync([FromBody] FlightPlannerSimpleViewModel Flight)
    {
        try
        {
            var repo = new PlannerRepository();
            bool isValid = await repo.ValidateAsync(Obj: Flight);
            
            if (isValid == false)
                return BadRequest("La validación falló.");
            
            await repo.UpdateFlightPlanAsync(ViewModel: Flight);
            return new JsonResult(new
            {
                success = true,
                ID = Flight.ID,
            });
        }
        catch (ApplicationException aex)
        {
            return new JsonResult(new
            {
                success = false,
                message = aex.Message
            });
        }
        catch (Exception)
        {
            return new JsonResult(new
            {
                success = false,
                message = "Ocurrió un error inesperado al guardar el plan de vuelo."
            });
        }
    }
}