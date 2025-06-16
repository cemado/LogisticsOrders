using ClosedXML.Excel;
using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Application.Reports;

namespace LogisticsOrders.Infrastructure.Reports;

public class ExcelReportGenerator
{
    public void GenerateReport()
    {
        // Implementación de ejemplo para generar un reporte de Excel vacío
    }

    public byte[] GenerateOrdersByClientReport(IEnumerable<Order> orders)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Órdenes");

        // Encabezados
        worksheet.Cell(1, 1).Value = "Cliente";
        worksheet.Cell(1, 2).Value = "Producto";
        worksheet.Cell(1, 3).Value = "Cantidad";
        worksheet.Cell(1, 4).Value = "Origen";
        worksheet.Cell(1, 5).Value = "Destino";
        worksheet.Cell(1, 6).Value = "Distancia (km)";
        worksheet.Cell(1, 7).Value = "Costo";

        int row = 2;
        foreach (var order in orders)
        {
            worksheet.Cell(row, 1).Value = order.Client;
            worksheet.Cell(row, 2).Value = order.Product;
            worksheet.Cell(row, 3).Value = order.Quantity;
            worksheet.Cell(row, 4).Value = $"{order.Origin.Latitude}, {order.Origin.Longitude}";
            worksheet.Cell(row, 5).Value = $"{order.Destination.Latitude}, {order.Destination.Longitude}";
            worksheet.Cell(row, 6).Value = order.DistanceKm;
            worksheet.Cell(row, 7).Value = order.EstimatedCost;
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] GenerateOrdersByDistanceIntervalReport(IEnumerable<OrdersByIntervalDto> report)
    {
        using var workbook = new ClosedXML.Excel.XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Reporte");

        // Encabezados
        worksheet.Cell(1, 1).Value = "Cliente";
        worksheet.Cell(1, 2).Value = "1-50 km";
        worksheet.Cell(1, 3).Value = "51-200 km";
        worksheet.Cell(1, 4).Value = "201-500 km";
        worksheet.Cell(1, 5).Value = "501-1000 km";

        int row = 2;
        foreach (var item in report)
        {
            worksheet.Cell(row, 1).Value = item.Client;
            worksheet.Cell(row, 2).Value = item.Intervals.First(i => i.Interval == "1-50 km").Count;
            worksheet.Cell(row, 3).Value = item.Intervals.First(i => i.Interval == "51-200 km").Count;
            worksheet.Cell(row, 4).Value = item.Intervals.First(i => i.Interval == "201-500 km").Count;
            worksheet.Cell(row, 5).Value = item.Intervals.First(i => i.Interval == "501-1000 km").Count;
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}