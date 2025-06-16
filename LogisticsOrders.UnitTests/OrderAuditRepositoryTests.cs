using Xunit;
using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Infrastructure.Data;
using LogisticsOrders.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace LogisticsOrders.UnitTests;

public class OrderAuditRepositoryTests
{
    private LogisticsDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LogisticsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LogisticsDbContext(options);
    }

    [Fact]
    public void Add_And_GetByOrderId_Works()
    {
        var context = GetDbContext();
        var repo = new OrderAuditRepository(context);

        var audit = new OrderAudit
        {
            OrderId = 1,
            Action = "Creación",
            User = "admin",
            Timestamp = DateTime.UtcNow,
            Details = "Orden creada"
        };

        repo.Add(audit);

        var audits = repo.GetByOrderId(1).ToList();
        Assert.Single(audits);
        Assert.Equal("Creación", audits[0].Action);
    }
}