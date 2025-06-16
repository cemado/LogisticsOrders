using LogisticsOrders.Application.Reports;
using LogisticsOrders.Infrastructure.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsOrders.API.Pages.Reports;

public class ByDistanceIntervalModel : PageModel
{
    private readonly OrdersByDistanceIntervalReportService _reportService;
    private readonly ExcelReportGenerator _excelReportGenerator;

    public ByDistanceIntervalModel(
        OrdersByDistanceIntervalReportService reportService,
        ExcelReportGenerator excelReportGenerator)
    {
        _reportService = reportService;
        _excelReportGenerator = excelReportGenerator;
    }

    public IEnumerable<OrdersByIntervalDto>? Report { get; set; }

    public void OnGet()
    {
        Report = _reportService.GetReport();
    }

    public IActionResult OnGetDownloadExcel()
    {
        var report = _reportService.GetReport();
        var fileBytes = _excelReportGenerator.GenerateOrdersByDistanceIntervalReport(report);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReportePorIntervalos.xlsx");
    }
}