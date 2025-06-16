using Xunit;
using LogisticsOrders.Domain.Entities;

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
            CreatedAt = DateTime.Today,
            EstimatedCost = 100
        };

        Assert.Equal("Cliente de prueba", order.Client);
    }
}