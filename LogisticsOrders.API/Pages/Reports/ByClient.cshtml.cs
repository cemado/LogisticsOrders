using LogisticsOrders.Application.Reports;
using LogisticsOrders.Infrastructure.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsOrders.API.Pages.Reports;

public class ByClientModel : PageModel
{
    private readonly OrdersByClientReportService _reportService;
    private readonly ExcelReportGenerator _excelReportGenerator;

    public ByClientModel(
        OrdersByClientReportService reportService,
        ExcelReportGenerator excelReportGenerator)
    {
        _reportService = reportService;
        _excelReportGenerator = excelReportGenerator;
    }

    [BindProperty(SupportsGet = true)]
    public string? ClientName { get; set; }

    public IEnumerable<LogisticsOrders.Domain.Entities.Order>? Orders { get; set; }

    public void OnGet()
    {
        if (!string.IsNullOrWhiteSpace(ClientName))
        {
            Orders = _reportService.GetOrdersByClient(ClientName);
        }
    }

    public IActionResult OnGetDownloadExcel()
    {
        if (string.IsNullOrWhiteSpace(ClientName))
            return RedirectToPage();

        var orders = _reportService.GetOrdersByClient(ClientName);
        if (orders == null || !orders.Any())
        {
            TempData["ErrorMessage"] = "No hay datos para exportar.";
            return RedirectToPage(new { ClientName });
        }

        var fileBytes = _excelReportGenerator.GenerateOrdersByClientReport(orders);

        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Ordenes_{ClientName}.xlsx");
    }
}