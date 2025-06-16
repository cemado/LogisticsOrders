using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace LogisticsOrders.IntegrationTests;

public class OrdersEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrdersEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_CreateOrderPage_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/Orders/Create");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Crear Orden", content, System.StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Post_CreateOrder_ReturnsRedirectOrValidation()
    {
        // Arrange
        var formData = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("Order.Client", "ClienteTest"),
            new KeyValuePair<string, string>("Order.Product", "Producto1"),
            new KeyValuePair<string, string>("Order.Quantity", "1"),
            new KeyValuePair<string, string>("Order.OriginLat", "10"),
            new KeyValuePair<string, string>("Order.OriginLng", "20"),
            new KeyValuePair<string, string>("Order.DestLat", "30"),
            new KeyValuePair<string, string>("Order.DestLng", "40")
        });

        // Act
        var response = await _client.PostAsync("/Orders/Create", formData);

        // Assert
        // Puede ser Redirect (302) si la orden es válida, o 200 si hay error de validación
        Assert.True(
            response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.Redirect
        );
    }

    [Fact]
    public async Task Get_OrdersByClient_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/Orders/ByClient?ClientName=ClienteTest");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Órdenes por Cliente", content, System.StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Get_Dashboard_ReturnsSuccessAndChart()
    {
        // Act
        var response = await _client.GetAsync("/Dashboard/Index");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Dashboard", content, System.StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ordersChart", content); // Verifica que el canvas del gráfico esté presente
    }

    [Fact]
    public async Task Get_ReportByClient_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/Reports/ByClient");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Reporte por Cliente", content, System.StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Get_ReportByDistanceInterval_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/Reports/ByDistanceInterval");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Intervalo de Distancia", content, System.StringComparison.OrdinalIgnoreCase);
    }
}