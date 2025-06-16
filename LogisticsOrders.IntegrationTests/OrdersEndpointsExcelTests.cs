using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace LogisticsOrders.IntegrationTests;

public class OrdersEndpointsExcelTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrdersEndpointsExcelTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_DownloadExcel_ReturnsExcelFile()
    {
        // Arrange: Asegúrate de que haya datos para exportar, o ajusta el test según tu lógica
        var url = "/Orders/ByClient?ClientName=ClienteTest";
        await _client.GetAsync(url); // Opcional: fuerza la generación de datos

        // Act
        var response = await _client.GetAsync("/Orders/ByClient?handler=DownloadExcel&ClientName=ClienteTest");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            response.Content.Headers.ContentType?.MediaType);
        var content = await response.Content.ReadAsByteArrayAsync();
        Assert.True(content.Length > 0);
    }
}