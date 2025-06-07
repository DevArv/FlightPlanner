using FlightPlanner.Repositories;
using FlightPlanner.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlightPlanner.Pages.Flight;

[IgnoreAntiforgeryToken]
public class Details : PageModel
{
    public FlightPlannerDetailsViewModel Record { get; set; } = new();
    
    public async Task<IActionResult> OnGet(Guid ID)
    {
        var repo = new PlannerRepository();
        try
        {
            Record = await repo.GetFlightPlanAsync(ID: ID);
            return Page();
        }
        catch (ApplicationException aex)
        {
            return RedirectToPage("/Index", new { error = aex.Message });
        }
    }
    
    public class DeleteRequest
    {
        public Guid ID { get; set; }
    }

    public async Task<IActionResult> OnPostDeleteAsync([FromBody] DeleteRequest Request)
    {
        var repo = new PlannerRepository();
        try
        {
            await repo.DeleteFlightPlanAsync(ID: Request.ID);
            return new JsonResult(new { success = true });
        }
        catch (ApplicationException aex)
        {
            return new JsonResult(new { success = false, message = aex.Message });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = "Ocurrió un error inesperado al eliminar el plan de vuelo", ex });
        }
    }
}