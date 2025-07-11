using System.ComponentModel.DataAnnotations;
namespace FlightPlanner.ViewModels;

public class LoginViewModel
{
    [Required]
    public string UserName { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}