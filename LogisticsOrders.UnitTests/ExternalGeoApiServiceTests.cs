using Xunit;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LogisticsOrders.Infrastructure.Services;
using Moq.Protected;

namespace LogisticsOrders.UnitTests;

public class ExternalGeoApiServiceTests
{
    [Fact]
    public async Task ValidateAddressAsync_ReturnsTrue_OnSuccess()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected() // Use Protected() to access protected members  
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        var httpClient = new HttpClient(handlerMock.Object);
        var service = new ExternalGeoApiService(httpClient);

        var result = await service.ValidateAddressAsync("Av. Principal 123");
        Assert.True(result);
    }
}