using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsOrders.API.Pages.Orders;

public class DetailsModel : PageModel
{
    private readonly IOrderRepository _orderRepository;

    public DetailsModel(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Order? Order { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public IActionResult OnGet()
    {
        Order = _orderRepository.GetById(Id);
        if (Order == null)
            return NotFound();

        return Page();
    }
}