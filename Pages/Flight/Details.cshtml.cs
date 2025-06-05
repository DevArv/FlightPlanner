using FlightPlanner.Repositories;
using FlightPlanner.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlightPlanner.Pages.Flight;

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
}