using LogisticsOrders.Domain.ValueObjects;

namespace LogisticsOrders.Domain.Services;

public static class DistanceCalculator
{
    public static double HaversineKm(GeoPoint p1, GeoPoint p2)
    {
        const double R = 6371;
        var lat1 = DegreesToRadians(p1.Latitude);
        var lat2 = DegreesToRadians(p2.Latitude);
        var dLat = DegreesToRadians(p2.Latitude - p1.Latitude);
        var dLon = DegreesToRadians(p2.Longitude - p1.Longitude);

        var a = Math.Sin(dLat/2) * Math.Sin(dLat/2) +
                Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(dLon/2) * Math.Sin(dLon/2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));
        return R * c;
    }

    private static double DegreesToRadians(double deg) => deg * Math.PI / 180.0;
}