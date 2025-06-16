using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Interfaces;
using static LogisticsOrders.Application.Reports.OrdersByIntervalDto;

namespace LogisticsOrders.Application.Reports;

public class OrdersByDistanceIntervalReportService
{
    private readonly IOrderRepository _orderRepository;

    public OrdersByDistanceIntervalReportService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public IEnumerable<OrdersByIntervalDto> GetReport()
    {
        var orders = _orderRepository.GetAll();

        var intervals = new[]
        {
            new { Min = 1.0, Max = 50.0, Label = "1-50 km" },
            new { Min = 51.0, Max = 200.0, Label = "51-200 km" },
            new { Min = 201.0, Max = 500.0, Label = "201-500 km" },
            new { Min = 501.0, Max = 1000.0, Label = "501-1000 km" }
        };

        return orders
            .GroupBy(o => o.Client)
            .Select(g => new OrdersByIntervalDto
            {
                Client = g.Key,
                Intervals = intervals.Select(i => new IntervalCount
                {
                    Interval = i.Label,
                    Count = g.Count(o => o.DistanceKm >= i.Min && o.DistanceKm <= i.Max)
                }).ToList()
            });
    }
}
