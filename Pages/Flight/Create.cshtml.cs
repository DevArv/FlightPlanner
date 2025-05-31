using FlightPlanner.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlightPlanner.Pages.Flight;

public class Create : PageModel
{
    public List<Planner> Planner { get; set; } = new();
    
    public void OnGet()
    {
        
    }
}