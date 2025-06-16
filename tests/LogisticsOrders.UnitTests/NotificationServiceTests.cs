using Xunit;
using LogisticsOrders.Infrastructure.Services;

namespace LogisticsOrders.UnitTests;

public class NotificationServiceTests
{
    [Fact]
    public void Notify_DoesNotThrow()
    {
        var service = new NotificationService();
        service.Notify("Mensaje de prueba");
        // No se espera excepción, es un placeholder
    }
}