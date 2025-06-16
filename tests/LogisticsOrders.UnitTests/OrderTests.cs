using Xunit;
using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.ValueObjects;

namespace LogisticsOrders.UnitTests;

public class OrderTests
{
    [Fact]
    public void CanCreateOrder()
    {
        var order = new Order
        {
            Id = 1,
            Client = "Cliente de prueba",
            Product = "Producto X",
            Quantity = 2,
            Origin = new GeoPoint(10, 20),
            Destination = new GeoPoint(30, 40),
            DistanceKm = 100,
            EstimatedCost = 300,
            CreatedAt = DateTime.Today
        };

        Assert.Equal("Cliente de prueba", order.Client);
        Assert.Equal("Producto X", order.Product);
        Assert.Equal(2, order.Quantity);
    }
}