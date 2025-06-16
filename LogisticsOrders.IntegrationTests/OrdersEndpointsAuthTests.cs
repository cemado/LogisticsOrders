using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace LogisticsOrders.IntegrationTests;

public class OrdersEndpointsAuthTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrdersEndpointsAuthTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // No PostConfigure here, as we are using a custom authentication handler
            });
        }).CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Get_EditOrder_ProtectedEndpoint_ReturnsSuccessForAdmin()
    {
        // Act
        var response = await _client.GetAsync("/Orders/Edit/1"); // Ajusta la URL según tu routing

        // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.NotFound // Puede ser NotFound si no existe la orden
        );
    }

    [Fact]
    public async Task Get_EditOrder_ProtectedEndpoint_ReturnsUnauthorizedForAnonymous()
    {
        // Arrange: Cliente sin autenticación
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/Orders/Edit/1");

        // Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode); // Redirige a login o acceso denegado
    }
}