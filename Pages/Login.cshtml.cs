using System.Security.Claims;
using FlightPlanner.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlightPlanner.Pages;

[IgnoreAntiforgeryToken]
public class Login : PageModel
{
    [BindProperty]
    public LoginViewModel Credentials { get; set; } = new();

    public string? Error { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var config = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var user = config["Credentials:UserName"];
        var pass = config["Credentials:Password"];

        if (Credentials.UserName == user && Credentials.Password == pass)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, Credentials.UserName) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            return RedirectToPage("/Index");
        }

        Error = "Credenciales incorrectas.";
        return Page();
    }
}