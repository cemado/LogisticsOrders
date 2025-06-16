using LogisticsOrders.Application.Orders;
using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsOrders.API.Pages.Orders;

public class DeleteModel : PageModel
{
    private readonly IOrderRepository _orderRepository;
    private readonly DeleteOrderHandler _handler;

    public DeleteModel(IOrderRepository orderRepository, DeleteOrderHandler handler)
    {
        _orderRepository = orderRepository;
        _handler = handler;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public string? ClientName { get; set; }
    public Order? Order { get; set; }

    public IActionResult OnGet()
    {
        Order = _orderRepository.GetById(Id);
        if (Order == null)
            return NotFound();

        ClientName = Order.Client;
        return Page();
    }

    public IActionResult OnPost()
    {
        try
        {
            _handler.Delete(Id, User.Identity?.Name);
            TempData["SuccessMessage"] = "Orden eliminada correctamente.";
            return RedirectToPage("ByClient", new { ClientName });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return Page();
        }
    }
}