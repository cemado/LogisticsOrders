using Xunit;
using Moq;
using LogisticsOrders.Application.Orders;
using LogisticsOrders.Domain.Entities;
using LogisticsOrders.Domain.Interfaces;
using System;

namespace LogisticsOrders.UnitTests;

public class DeleteOrderHandlerTests
{
    [Fact]
    public void Delete_ValidOrder_DeletesOrder()
    {
        // Arrange
        var repoMock = new Mock<IOrderRepository>();
        var auditRepoMock = new Mock<IOrderAuditRepository>();

        var order = new Order { Id = 1, Client = "Cliente1" };
        repoMock.Setup(r => r.GetById(1)).Returns(order);

        var handler = new DeleteOrderHandler(repoMock.Object, auditRepoMock.Object);

        // Act
        handler.Delete(1, "usuarioTest");

        // Assert
        repoMock.Verify(r => r.Delete(1), Times.Once);
        auditRepoMock.Verify(a => a.Add(It.IsAny<OrderAudit>()), Times.Once);
    }
}