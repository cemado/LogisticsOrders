using LogisticsOrders.Application.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsOrders.API.Pages.Orders;

public class CreateModel : PageModel
{
    private readonly CreateOrderHandler _handler;

    public CreateModel(CreateOrderHandler handler)
    {
        _handler = handler;
    }

    [BindProperty]
    public CreateOrderCommand Order { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            await _handler.Handle(Order, User.Identity?.Name);
            TempData["SuccessMessage"] = "Orden creada correctamente.";
            return RedirectToPage("ByClient", new { ClientName = Order.Client });
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }
    }
}