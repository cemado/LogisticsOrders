using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsOrders.API.Pages.Account;

public class LoginModel : PageModel
{
    [BindProperty]
    public string? Username { get; set; }
    [BindProperty]
    public string? Password { get; set; }
    public string? ErrorMessage { get; set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        // Lógica de autenticación de ejemplo
        if (Username == "admin" && Password == "admin")
        {
            // Aquí deberías autenticar al usuario y asignar roles
            return RedirectToPage("/Dashboard/Index");
        }
        ErrorMessage = "Usuario o contraseña incorrectos.";
        return Page();
    }
}