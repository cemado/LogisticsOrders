using System.Net.Http;
using System.Threading.Tasks;
using LogisticsOrders.Application.Interfaces;

namespace LogisticsOrders.Infrastructure.Services;

public class ExternalGeoApiService : IExternalGeoApiService
{
    private readonly HttpClient _httpClient;

    public ExternalGeoApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ValidateAddressAsync(string address)
    {
        var response = await _httpClient.GetAsync("https://api.ejemplo.com/validate?address=" + address);
        return response.IsSuccessStatusCode;
    }
}