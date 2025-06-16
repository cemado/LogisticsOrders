namespace LogisticsOrders.Domain.Services;

public static class OrderCostCalculator
{
    public static decimal CalculateCost(double distanceKm)
    {
        if (distanceKm < 1 || distanceKm > 1000)
            throw new ArgumentOutOfRangeException(nameof(distanceKm), "La distancia debe estar entre 1 y 1000 km.");

        if (distanceKm <= 50) return 100m;
        if (distanceKm <= 200) return 300m;
        if (distanceKm <= 500) return 1000m;
        return 1500m;
    }
}