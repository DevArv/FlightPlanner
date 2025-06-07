using FlightPlanner.Repositories;
using FlightPlanner.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlightPlanner.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    
    public List<FlightPlannerDetailsViewModel> Flights { get; set; } = new();
    
    [BindProperty(SupportsGet = true)]
    public string? Error { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        var repo = new PlannerRepository();
        Flights = await repo.GetTableFlightPlansAsync();
    }
}
