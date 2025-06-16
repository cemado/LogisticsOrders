using Xunit;
using Moq;
using LogisticsOrders.Application.Dashboard;
using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Interfaces;
using System.Collections.Generic;
using System;

namespace LogisticsOrders.UnitTests;

public class DashboardServiceTests
{
    [Fact]
    public void GetSummary_ReturnsCorrectCounts()
    {
        var repoMock = new Mock<IOrderRepository>();
        repoMock.Setup(r => r.GetAll()).Returns(new List<Order>
        {
            new Order { Client = "A" },
            new Order { Client = "B" },
            new Order { Client = "A" }
        });

        var service = new DashboardService(repoMock.Object);
        var summary = service.GetSummary();

        Assert.Equal(3, summary.TotalOrders);
        Assert.Equal(2, summary.UniqueClients);
    }

    [Fact]
    public void GetOrdersCountByMonth_ReturnsMonths()
    {
        var now = DateTime.UtcNow;
        var repoMock = new Mock<IOrderRepository>();
        repoMock.Setup(r => r.GetAll()).Returns(new List<Order>
        {
            new Order { CreatedAt = now.AddMonths(-1) },
            new Order { CreatedAt = now.AddMonths(-1) },
            new Order { CreatedAt = now }
        });

        var service = new DashboardService(repoMock.Object);
        var result = service.GetOrdersCountByMonth(2);

        Assert.Equal(2, result.Count);
        Assert.Equal(2, result[0].Count + result[1].Count);
    }
}