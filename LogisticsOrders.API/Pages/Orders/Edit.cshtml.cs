using LogisticsOrders.Application.Dashboard;
using LogisticsOrders.Application.Interfaces;
using LogisticsOrders.Application.Orders;
using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsOrders.API.Pages.Orders;

public class EditModel : PageModel
{
    private readonly IOrderRepository _orderRepository;
    private readonly IExternalGeoApiService _geoApiService;
    private readonly DashboardService _dashboardService;

    public EditModel(
        IOrderRepository orderRepository,
        IExternalGeoApiService geoApiService,
        DashboardService dashboardService)
    {
        _orderRepository = orderRepository;
        _geoApiService = geoApiService;
        _dashboardService = dashboardService;
    }

    [BindProperty]
    public CreateOrderCommand Order { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public string? ErrorMessage { get; set; }
    public DashboardSummary? Summary { get; private set; }
    public List<string>? Months { get; private set; }
    public List<int>? OrdersPerMonth { get; private set; }

    public IActionResult OnGet()
    {
        Summary = _dashboardService.GetSummary();

        // Obtener datos de órdenes por mes (últimos 6 meses)
        var ordersByMonth = _dashboardService.GetOrdersCountByMonth(6);

        Months = ordersByMonth.Select(x => x.MonthName).ToList();
        OrdersPerMonth = ordersByMonth.Select(x => x.Count).ToList();

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            ErrorMessage = "Por favor, corrija los errores en el formulario.";
            return Page();
        }
        try
        {
            var handler = new EditOrderHandler(_orderRepository, _geoApiService);
            handler.EditAsync(Id, Order).GetAwaiter().GetResult();
            return RedirectToPage("Index");
        }
        catch (InvalidOperationException ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }
    }
}

