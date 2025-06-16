using System.Linq;
using LogisticsOrders.Domain.Interfaces;
using System.Globalization;

namespace LogisticsOrders.Application.Dashboard;

public class DashboardService
{
    private readonly IOrderRepository _orderRepository;

    public DashboardService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public DashboardSummary GetSummary()
    {
        var orders = _orderRepository.GetAll();
        return new DashboardSummary
        {
            TotalOrders = orders.Count(),
            UniqueClients = orders.Select(o => o.Client).Distinct().Count()
        };
    }

    public List<OrdersByMonthDto> GetOrdersCountByMonth(int monthsBack)
    {
        var orders = _orderRepository.GetAll();
        var now = DateTime.UtcNow;
        var start = now.AddMonths(-monthsBack + 1);

        var grouped = orders
            .Where(o => o.CreatedAt >= start)
            .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
            .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
            .Select(g => new OrdersByMonthDto
            {
                MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy", new CultureInfo("es-ES")),
                Count = g.Count()
            })
            .ToList();

        // Asegura que todos los meses estén presentes aunque no haya órdenes
        var result = new List<OrdersByMonthDto>();
        for (int i = 0; i < monthsBack; i++)
        {
            var date = now.AddMonths(-monthsBack + 1 + i);
            var monthName = date.ToString("MMMM yyyy", new CultureInfo("es-ES"));
            var found = grouped.FirstOrDefault(x => x.MonthName == monthName);
            result.Add(new OrdersByMonthDto
            {
                MonthName = monthName,
                Count = found?.Count ?? 0
            });
        }
        return result;
    }
}

public class DashboardSummary
{
    public int TotalOrders { get; set; }
    public int UniqueClients { get; set; }
}

public class OrdersByMonthDto
{
    public string MonthName { get; set; } = "";
    public int Count { get; set; }
}