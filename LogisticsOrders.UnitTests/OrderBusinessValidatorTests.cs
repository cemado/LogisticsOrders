using Xunit;
using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Services;
using LogisticsOrders.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace LogisticsOrders.UnitTests;

public class OrderBusinessValidatorTests
{
    [Fact]
    public void IsProductValid_ReturnsTrue_ForValidProduct()
    {
        var validProducts = new[] { "Producto1", "Producto2" };
        Assert.True(OrderBusinessValidator.IsProductValid("Producto1", validProducts));
    }

    [Fact]
    public void IsProductValid_ReturnsFalse_ForInvalidProduct()
    {
        var validProducts = new[] { "Producto1", "Producto2" };
        Assert.False(OrderBusinessValidator.IsProductValid("ProductoX", validProducts));
    }

    [Fact]
    public void IsDuplicateOrder_ReturnsTrue_ForDuplicate()
    {
        var order = new Order
        {
            Client = "Cliente1",
            Product = "Producto1",
            Origin = new GeoPoint(1, 1),
            Destination = new GeoPoint(2, 2),
            CreatedAt = DateTime.Today
        };
        var existing = new List<Order> { order };
        var newOrder = new Order
        {
            Client = "Cliente1",
            Product = "Producto1",
            Origin = new GeoPoint(1, 1),
            Destination = new GeoPoint(2, 2),
            CreatedAt = DateTime.Today
        };
        Assert.True(OrderBusinessValidator.IsDuplicateOrder(newOrder, existing));
    }
}