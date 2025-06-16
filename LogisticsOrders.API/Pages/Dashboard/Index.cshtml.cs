using LogisticsOrders.Application.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace LogisticsOrders.API.Pages.Dashboard;

public class IndexModel : PageModel
{
    private readonly DashboardService _dashboardService;

    public IndexModel(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public DashboardSummary? Summary { get; set; }
    public List<string> Months { get; set; } = new();
    public List<int> OrdersPerMonth { get; set; } = new();

    public IActionResult OnGet()
    {
        Summary = _dashboardService.GetSummary();

        // Obtener datos de órdenes por mes (últimos 6 meses)
        var ordersByMonth = _dashboardService.GetOrdersCountByMonth(6);

        Months = ordersByMonth.Select(x => x.MonthName).ToList();
        OrdersPerMonth = ordersByMonth.Select(x => x.Count).ToList();

        return Page();
    }
}