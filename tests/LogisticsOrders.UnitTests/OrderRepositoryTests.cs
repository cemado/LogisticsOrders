using System;
using System.Linq;
using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.ValueObjects;
using LogisticsOrders.Infrastructure.Data;
using LogisticsOrders.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LogisticsOrders.UnitTests;

public class OrderRepositoryTests
{
    private LogisticsDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LogisticsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LogisticsDbContext(options);
    }

    [Fact]
    public void AddAndGetOrder_Works()
    {
        var context = GetDbContext();
        var repo = new OrderRepository(context);

        var order = new Order
        {
            Client = "Cliente1",
            Product = "Producto1",
            Quantity = 1,
            Origin = new GeoPoint(1, 1),
            Destination = new GeoPoint(2, 2),
            DistanceKm = 10,
            EstimatedCost = 100,
            CreatedAt = DateTime.UtcNow
        };

        repo.Add(order);

        var orders = repo.GetAll().ToList();
        Assert.Single(orders);
        Assert.Equal("Cliente1", orders[0].Client);
    }

    [Fact]
    public void DeleteOrder_Works()
    {
        var context = GetDbContext();
        var repo = new OrderRepository(context);

        var order = new Order
        {
            Client = "Cliente2",
            Product = "Producto2",
            Quantity = 2,
            Origin = new GeoPoint(1, 1),
            Destination = new GeoPoint(2, 2),
            DistanceKm = 20,
            EstimatedCost = 200,
            CreatedAt = DateTime.UtcNow
        };

        repo.Add(order);
        var id = order.Id;
        repo.Delete(id);

        Assert.Empty(repo.GetAll());
    }

    [Fact]
    public void GetByClientWithFilters_Works()
    {
        var context = GetDbContext();
        var repo = new OrderRepository(context);

        var order1 = new Order
        {
            Client = "ClienteFiltro",
            Product = "ProductoFiltro",
            Quantity = 1,
            Origin = new GeoPoint(1, 1),
            Destination = new GeoPoint(2, 2),
            DistanceKm = 15,
            EstimatedCost = 150,
            CreatedAt = new DateTime(2024, 1, 1)
        };
        var order2 = new Order
        {
            Client = "ClienteFiltro",
            Product = "OtroProducto",
            Quantity = 2,
            Origin = new GeoPoint(1, 1),
            Destination = new GeoPoint(2, 2),
            DistanceKm = 25,
            EstimatedCost = 250,
            CreatedAt = new DateTime(2024, 2, 1)
        };

        repo.Add(order1);
        repo.Add(order2);

        var result = repo.GetByClientWithFilters("ClienteFiltro", "ProductoFiltro", new DateTime(2023, 12, 31), new DateTime(2024, 1, 2));
        Assert.Single(result);
        Assert.Equal("ProductoFiltro", result.First().Product);
    }
}