using Xunit;
using Moq;
using LogisticsOrders.Application.Orders;
using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Interfaces;
using LogisticsOrders.Infrastructure.Services;
using LogisticsOrders.Domain.ValueObjects;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace LogisticsOrders.UnitTests;

public class EditOrderHandlerTests
{
    [Fact]
    public async Task EditAsync_ValidOrder_UpdatesOrder()
    {
        // Arrange
        var repoMock = new Mock<IOrderRepository>();
        var geoApiMock = new Mock<ExternalGeoApiService>(null as HttpClient);
        var auditRepoMock = new Mock<IOrderAuditRepository>();

        var existingOrder = new Order
        {
            Id = 1,
            Client = "Cliente1",
            Product = "Producto1",
            Quantity = 1,
            Origin = new GeoPoint(10, 20),
            Destination = new GeoPoint(30, 40),
            DistanceKm = 100,
            EstimatedCost = 300,
            CreatedAt = DateTime.UtcNow
        };

        repoMock.Setup(r => r.GetById(1)).Returns(existingOrder);
        repoMock.Setup(r => r.GetByClientWithFilters(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(new List<Order> { existingOrder });
        geoApiMock.Setup(g => g.ValidateAddressAsync(It.IsAny<string>())).ReturnsAsync(true);

        var handler = new EditOrderHandler(repoMock.Object, geoApiMock.Object, auditRepoMock.Object);

        var cmd = new CreateOrderCommand
        {
            Client = "Cliente1",
            Product = "Producto1",
            Quantity = 2,
            OriginLat = 11,
            OriginLng = 21,
            DestLat = 31,
            DestLng = 41
        };

        // Act
        await handler.EditAsync(1, cmd, "usuarioTest");

        // Assert
        repoMock.Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
        auditRepoMock.Verify(a => a.Add(It.IsAny<OrderAudit>()), Times.Once);
    }
}