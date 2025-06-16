using System.Threading.Tasks;
using Xunit;
using Moq;
using LogisticsOrders.Application.Orders;
using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Interfaces;
using LogisticsOrders.Infrastructure.Services;
using LogisticsOrders.Domain.ValueObjects;
using System.Collections.Generic;
using System;

namespace LogisticsOrders.UnitTests;

public class CreateOrderHandlerTests
{
    [Fact]
    public async Task Handle_ValidOrder_CreatesOrder()
    {
        // Arrange
        var repoMock = new Mock<IOrderRepository>();
        var geoApiMock = new Mock<ExternalGeoApiService>(null as HttpClient);
        var auditRepoMock = new Mock<IOrderAuditRepository>();

        geoApiMock.Setup(g => g.ValidateAddressAsync(It.IsAny<string>())).ReturnsAsync(true);
        repoMock.Setup(r => r.GetByClientWithFilters(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(new List<Order>());

        var handler = new CreateOrderHandler(repoMock.Object, geoApiMock.Object, auditRepoMock.Object);

        var cmd = new CreateOrderCommand
        {
            Client = "Cliente1",
            Product = "Producto1",
            Quantity = 1,
            OriginLat = 10,
            OriginLng = 20,
            DestLat = 30,
            DestLng = 40
        };

        // Act
        var result = await handler.Handle(cmd, "usuarioTest");

        // Assert
        repoMock.Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
        auditRepoMock.Verify(a => a.Add(It.IsAny<OrderAudit>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidAddress_ThrowsException()
    {
        // Arrange
        var repoMock = new Mock<IOrderRepository>();
        var geoApiMock = new Mock<ExternalGeoApiService>(null as HttpClient);
        geoApiMock.Setup(g => g.ValidateAddressAsync(It.IsAny<string>())).ReturnsAsync(false);

        var handler = new CreateOrderHandler(repoMock.Object, geoApiMock.Object);

        var cmd = new CreateOrderCommand
        {
            Client = "Cliente1",
            Product = "Producto1",
            Quantity = 1,
            OriginLat = 10,
            OriginLng = 20,
            DestLat = 30,
            DestLng = 40
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(cmd));
    }
}