using System.Threading.Tasks;

namespace LogisticsOrders.Application.Interfaces;

public interface IExternalGeoApiService
{
    Task<bool> ValidateAddressAsync(string address);
}