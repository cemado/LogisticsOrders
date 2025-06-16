using LogisticsOrders.Application.Reports;
using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Interfaces;
using LogisticsOrders.Infrastructure.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;
using X.PagedList.Extensions;


namespace LogisticsOrders.API.Pages.Orders;

public class ByClientModel : PageModel
{
    private readonly OrdersByClientReportService _reportService;
    private readonly ExcelReportGenerator _excelReportGenerator;
    private readonly IOrderRepository _orderRepository;

    public ByClientModel(
        OrdersByClientReportService reportService,
        ExcelReportGenerator excelReportGenerator,
        IOrderRepository orderRepository)
    {
        _reportService = reportService;
        _excelReportGenerator = excelReportGenerator;
        _orderRepository = orderRepository;
    }

    [BindProperty(SupportsGet = true)]
    public string? ClientName { get; set; }

    [BindProperty(SupportsGet = true)]
    public new int Page { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Product { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? FromDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? ToDate { get; set; }

    public IPagedList<Order>? Orders { get; set; }
    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
        if (!string.IsNullOrWhiteSpace(ClientName))
        {
            var query = _reportService.GetOrdersByClient(ClientName).AsQueryable();

            if (!string.IsNullOrWhiteSpace(Product))
                query = query.Where(o => o.Product.Contains(Product));

            if (FromDate.HasValue)
                query = query.Where(o => o.CreatedAt >= FromDate.Value);

            if (ToDate.HasValue)
                query = query.Where(o => o.CreatedAt <= ToDate.Value);

            Orders = query.OrderByDescending(o => o.CreatedAt).ToPagedList(Page, 10);

            if (!Orders.Any())
                ErrorMessage = "No se encontraron órdenes para el cliente especificado.";
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