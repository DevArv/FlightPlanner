using FlightPlanner.Repositories;
using FlightPlanner.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlightPlanner.Pages.Flight;

[IgnoreAntiforgeryToken]
public class CreateModel : PageModel
{
    [BindProperty]
    public FlightPlannerSimpleViewModel Record { get; set; } = new();
    
    public IActionResult OnGet()
    {
        Record.Date = DateTime.UtcNow.Date;
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostSaveAsync([FromBody] FlightPlannerSimpleViewModel Flight)
    {
        try
        {
            var repo = new PlannerRepository();
            bool isValid = await repo.ValidateAsync(Obj: Flight);

            if (isValid == false)
                return BadRequest("La validación falló.");

            var ID = await repo.SaveFlightPlanAsync(ViewModel: Flight);
            return new JsonResult(new
            {
                success = true, 
                ID = ID
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
